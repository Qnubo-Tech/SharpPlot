using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SharpPlot.Canvas;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

namespace SharpPlot
{
    
    public enum ChartType
    {
        TwoDimensional = 2,
        ThreeDimensional = 3
    }

    public class DataSet
    {
        public int NumberOfDimensions { get; private set; }
        public Dictionary<AxisName, IEnumerable<double>> Data { get; set; }
        
        public DataSet(IEnumerable<double> x, IEnumerable<double> y)
        {
            NumberOfDimensions = Convert.ToInt32(ChartType.TwoDimensional);
            _initialiseData(new List<IEnumerable<double>>(){x, y});
        }
        
        public DataSet(
            IEnumerable<double> x,
            IEnumerable<double> y,
            IEnumerable<double> z
            )
        {
            NumberOfDimensions = Convert.ToInt32(ChartType.ThreeDimensional);
            _initialiseData(new List<IEnumerable<double>>(){x, y, z});
        }

        private void _initialiseData(IEnumerable<IEnumerable<double>> data)
        {
            Data = new Dictionary<AxisName, IEnumerable<double>>(NumberOfDimensions);
            for (int k = 0; k < NumberOfDimensions; k++)
            {
                Data.Add((AxisName) k, data.ElementAtOrDefault(k));
            }
        }

        public IEnumerable<double> this[AxisName d] => Data[d];
    }
    
    
    public static class Gnuplot
    {
        #region Attributes
        private static string _gnuplotStr = "gnuplot";
        private static StreamWriter _gnuplotCmd;
        private static Process _gnuplotProcess;
        
        public static string WinDrive = "F";
        public static string WinBinFolder = @"Program Files\gnuplot\bin";
        public static string LinuxBinFolder = "/usr/local/bin";
        public static string OSXBinFolder = "/usr/local/bin";
        public static Axis Axis;
        public static Legend Legend;

        private static int _figureCounter = 0;
        private static Dictionary<int, Figure> _figuresDict = new Dictionary<int, Figure>();
        
        #endregion

        #region Properties

        private static PlotType PlotType { get; set; } = PlotType.Plot;

        private static string PlotInit => _getPlotInit();
        
        private static string UnsetColorBoxCommand => $"unset colorbox{Environment.NewLine}";
        #endregion

        private static int _getNextId()
        {
            _figureCounter++;
            return _figureCounter;
        }

        private static string _getPlotInit()
        {
            return Environment.NewLine + PlotType.ToString().ToLower();
        }

        public static void Start()
        {
            string file = $"{OSXBinFolder}/{_gnuplotStr}";
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                file = $"{LinuxBinFolder}/{_gnuplotStr}";
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                file = $"{OSXBinFolder}/{_gnuplotStr}";
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                file = $@"{WinDrive}:\{WinBinFolder}\{_gnuplotStr}.exe";
            }
            
            _gnuplotInit(file);
        }
        
        public static void Start(string gnuplotFullPath)
        {
            _gnuplotInit(file: gnuplotFullPath);
        }
        
        private static void _gnuplotInit(string file)
        {
            _gnuplotProcessInit(file);
            
            _gnuplotCmd = _gnuplotProcess.StandardInput;
            _gnuplotCmd.WriteLine(UnsetColorBoxCommand);

            Axis = new Axis();
            Legend = new Legend();
        }
        private static void _gnuplotProcessInit(string gnuplotFile)
        {   
            _gnuplotProcess = new Process
            {
                StartInfo =
                {
                    FileName = gnuplotFile,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    WindowStyle = ProcessWindowStyle.Normal
                }
            };
            
            _gnuplotProcess.Start();
        }
        
        public static void Exit()
        {
            _gnuplotCmd.Close();
            _gnuplotProcess.WaitForExit();
        }

        public static void WriteCommand(string command)
        {
            try
            {
                _gnuplotCmd.WriteLine(command);
            }
            catch
            {
                Console.WriteLine("Something happened! => Review you command");
            }
        }
        
        public static Figure GetFigure(int id)
        {
            return _figuresDict[id];
        }

        public static void SetPlotType(PlotType plotType)
        {
            PlotType = plotType;
        }

        public static void SetIsolineDensiy(double density)
        {
            WriteCommand($"set isosamples {density}");
        }

        public static void SetHidden3D()
        {
            WriteCommand($"set hidden3d");
        }

        private static bool _checkCommensurability(IEnumerable<IEnumerable<double>> z)
        {
            var lengths = z.Select(e => e.Count()).ToList();
            var commensurate = lengths.Aggregate(true, (current, t) => (current && (t == lengths.First())));
            if (!commensurate) {throw new ApplicationException($"[!] Shape: {string.Join(", ", lengths)}");}
            return true;
        }
        
        public static (int, TFigure) Plot<TFigure>(IEnumerable<double> x, IEnumerable<double> y) 
            where TFigure : Figure, new()
        {
            _checkCommensurability(new [] {x, y});
            var fig = new TFigure {ArrX = x.ToArray(), ArrY = y};
            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }
        
        public static (int, TFigure) Plot<TFigure>(IEnumerable<double> x, IEnumerable<double> y, IEnumerable<double> z) 
            where TFigure : Figure, new()
        {
            _checkCommensurability(new [] {x, y, z});
            var fig = new TFigure {ArrX = x, ArrY = y, ArrZ = z};
            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }
        
        public static (int, TFigure) Plot<TFigure>(string function) 
            where TFigure : Figure, new()
        {
            var fig = new TFigure(){ Properties = {Function = function}};
            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }
        
        public static (int, TFigure) Plot<TFigure>(DataSet ds) 
            where TFigure : Figure, new()
        {
            _checkCommensurability(new [] {ds[AxisName.X], ds[AxisName.Y]});
            var fig = new TFigure {ArrX = ds[AxisName.X], ArrY = ds[AxisName.Y]};
            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }

        public static (int, TFigure) Plot<TFigure>(IEnumerable<double> x, IEnumerable<double> y,
            string title, double size = 1, double width=1.0, DashType dashType=DashType.Solid,
            Marker marker = Marker.ColoredCircle, Color color = Color.Black)
            where TFigure : Figure, new()
        {
            _checkCommensurability(new [] {x, y});
            var fig = new TFigure {
                ArrX = x, 
                ArrY = y, 
                Properties =
                {
                    Color = color, Marker = marker, 
                    DashType = dashType, Size = size, 
                    Title = title, Width = width
                }
            };

            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }
        
        public static (int, TFigure) Plot<TFigure>(IEnumerable<double> x, IEnumerable<double> y, IEnumerable<double> z,
            string title, double size = 1, double width=1.0, DashType dashType=DashType.Solid,
            Marker marker = Marker.ColoredCircle, Color color = Color.Black)
            where TFigure : Figure, new()
        {
            _checkCommensurability(new [] {x, y, z});
            var fig = new TFigure {
                ArrX = x, 
                ArrY = y,
                ArrZ = z,
                Properties =
                {
                    Color = color, Marker = marker, 
                    DashType = dashType, Size = size, 
                    Title = title, Width = width
                }
            };

            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }

        public static (int, TFigure) Plot<TFigure>(DataSet ds,
            string title, double size = 1, double width = 1.0, DashType dashType = DashType.Solid,
            Marker marker = Marker.ColoredCircle, Color color = Color.Black)
            where TFigure : Figure, new()
        {
            _checkCommensurability(new[] {ds[AxisName.X], ds[AxisName.Y]});
            var fig = new TFigure {
                ArrX = ds[AxisName.X], 
                ArrY = ds[AxisName.Y], 
                Properties =
                {
                    Color = color, Marker = marker, 
                    DashType = dashType, Size = size, 
                    Title = title, Width = width
                }
            };

            var figId = _getNextId();
            _figuresDict.Add(figId, fig);
            return (figId, fig);
        }

        public static void CleanData()
        {
           _figuresDict.Clear();
           PlotType = PlotType.Plot;
           Axis = new Axis();
           Legend = new Legend();
           WriteCommand("reset session");
        }


        public static void SetAutoscale(string axes)
        {
            WriteCommand($"set autoscale {axes}{Environment.NewLine}");
        }
        
        
        private static void _plotUpdate()
        {
            _gnuplotCmd.Flush();
        }
        
        public static void Show()
        {
            var plotInit = _figuresDict.Aggregate(PlotInit, (current, idFigure) => current + (idFigure.Value.HeaderPlot + " ,"));

            plotInit += Environment.NewLine;
            WriteCommand(plotInit);

            foreach (var dataPoint in _figuresDict.SelectMany(idFigure => idFigure.Value.DataPoints))
            {
                WriteCommand(dataPoint);
            }
        }

        public static void Wait()
        {
            Console.WriteLine("Press any key + Enter to continue...");
            Console.ReadLine();
        }

        public static void Replot() 
        {
            WriteCommand("replot\n");
        }
        
    }
}