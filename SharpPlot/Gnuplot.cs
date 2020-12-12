using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using SharpPlot.Canvas;
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
        private static string _plotInit = Environment.NewLine + "plot";

        public static string WinDrive = "F";
        public static string WinBinFolder = @"Program Files\gnuplot\bin";
        public static string LinuxBinFolder = "/usr/local/bin";
        public static string OSXBinFolder = "/usr/local/bin";
        public static Axis Axis;

        private static int _figureCounter = 0;
        private static Dictionary<int, Figure> _figuresDict = new Dictionary<int, Figure>();
        
        #endregion

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
            _gnuplotCmd.WriteLine($"unset colorbox{Environment.NewLine}");

            Axis = new Axis();
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
        
        
        //TODO: Check x and y size before figure initialising
        public static int PlotScatter(IEnumerable<double> x, IEnumerable<double> y, string title, double size=1, 
            Marker marker=Marker.ColoredCircle, Color color=Color.Black)
        {
            _figureCounter++;
            var figId = _figureCounter;
            _figuresDict.Add(figId, new Scatter(x: x, y: y, title: title, size: size, marker: marker, color: color));
            return figId;
        }

        public static int PlotLine2D(IEnumerable<double> x, IEnumerable<double> y, 
            string title, double width=1.0, DashType dashType=DashType.Solid,  Color color=Color.Black)
        {
            _figureCounter++;
            var figId = _figureCounter;
            _figuresDict.Add(figId, new Line2D(x: x, y: y, 
                title: title, width: width, dashType: dashType, color: color));
            return figId;
        }
        
        public static int PlotLine2D(DataSet ds, 
            string title, double width=1.0, DashType dashType=DashType.Solid, Color color=Color.Black)
        {
            _figureCounter++;
            var figId = _figureCounter;
            _figuresDict.Add(figId, new Line2D(x: ds[AxisName.X], y: ds[AxisName.Y], 
                title: title, width: width, dashType: dashType,color: color));
            return figId;
        }

        public static void CleanData()
        {
           _figuresDict.Clear();
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
            var plotInit = _figuresDict.Aggregate(_plotInit, (current, idFigure) => current + (idFigure.Value.HeaderPlot + " ,"));

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