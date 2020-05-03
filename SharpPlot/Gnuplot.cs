using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
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
        public List<List<double>> Data { get; set; }
        
        public DataSet(ChartType chartType = ChartType.TwoDimensional)
        {
            NumberOfDimensions = Convert.ToInt32(chartType);
            _initialiseData();
        }

        private void _initialiseData()
        {

            if (2 == NumberOfDimensions)
            {
                Data = new List<List<double>>()
                {
                    new List<double>(),
                    new List<double>(),
                };
            }

            if (3 == NumberOfDimensions)
            {
                Data = new List<List<double>>()
                {
                    new List<double>(),
                    new List<double>(),
                    new List<double>()
                };
            }
        }
    }
    
    
    public static class Gnuplot
    {
        private static string GnuplotStr = "gnuplot";
        private static StreamWriter GnuplotCmd;
        private static Process GnuplotProcess;
        private static string _plotInit = Environment.NewLine + "plot";

        public static Axis Axis;

        private static List<Figure> _figures = new List<Figure>();

        public static void Start(string drive = "F", string rootFolder = @"Program Files\gnuplot\bin")
        {
            string file = $@"{drive}:\{rootFolder}\{GnuplotStr}.exe";
            _gnuplotInit(file: file);
        }
        public static void Start(string filePath)
        {
            _gnuplotInit(file: filePath);
        }
        
        private static void _gnuplotInit(string file)
        {
            _gnuplotProcessInit(file);
            
            GnuplotCmd = GnuplotProcess.StandardInput;
            GnuplotCmd.WriteLine($"unset colorbox{Environment.NewLine}");

            Axis = new Axis();
        }
        private static void _gnuplotProcessInit(string gnuplotFile)
        {   
            GnuplotProcess = new Process
            {
                StartInfo =
                {
                    FileName = gnuplotFile,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    WindowStyle = ProcessWindowStyle.Normal
                }
            };
            
            GnuplotProcess.Start();
        }
        
        public static void Exit()
        {
            GnuplotCmd.Close();
            GnuplotProcess.WaitForExit();
        }

        public static void WriteCommand(string command)
        {
            try
            {
                GnuplotCmd.WriteLine(command);
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
            GnuplotCmd.Flush();
        }
        
        public static void Show()
        {
            var plotInit = _plotInit;
            foreach (var figure in _figures)
            {
                plotInit += figure.HeaderPlot + " ,";
            }

            plotInit += Environment.NewLine;
            WriteCommand(plotInit);

            foreach (var figure in _figures)
            {
                foreach (var dataPoint in figure.DataPoints)
                {
                    WriteCommand(dataPoint);
                }
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