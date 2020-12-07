using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using SharpPlot.Canvas;

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
        private static string _gnuplotStr = "gnuplot";
        private static StreamWriter _gnuplotCmd;
        private static Process _gnuplotProcess;
        private static string _plotInit = Environment.NewLine + "plot";

        public static string WinDrive = "F";
        public static string WinBinFolder = @"Program Files\gnuplot\bin";
        public static string LinuxBinFolder = "/usr/local/bin";
        public static string OSXBinFolder = "/usr/local/bin";
        public static Axis Axis;

        private static List<Figure> _figures = new List<Figure>();
        
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
        
        
        
        //TODO: Check x and y size before figure initialising
        public static void PlotScatter(IEnumerable<double> x, IEnumerable<double> y, string title)
        {
            _figures.Add(new Scatter(x: x, y: y, title: title));
        }

        public static void PlotLine2D(IEnumerable<double> x, IEnumerable<double> y, string title)
        {
            _figures.Add(new Line2D(x: x, y: y, title:  title));
        }
        
        public static void PlotLine2D(DataSet ds, string title)
        {
            _figures.Add(new Line2D(x: ds[AxisName.X], y: ds[AxisName.Y], title:  title));
        }

        public static void CleanData()
        {
           _figures.Clear();
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
            var plotInit = _figures.Aggregate(_plotInit, (current, figure) => current + (figure.HeaderPlot + " ,"));

            plotInit += Environment.NewLine;
            WriteCommand(plotInit);

            foreach (var dataPoint in _figures.SelectMany(figure => figure.DataPoints))
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