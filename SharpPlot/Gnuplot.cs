using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace SharpPlot
{
    
    public enum ChartType
    {
        TwoDimensional = 2,
        ThreeDimensional = 3
    }

    public enum Axis
    {
        X,
        Y,
        Z,
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
        private static string _plotInit = Environment.NewLine + "plot '-' ";

        private static double[] _xRange;
        private static double[] _yRange;
        
        private static List<double> VecX { get; set; }
        private static List<double> VecY { get; set; }
        private static List<double> _vecZ { get; set; }
        
        private static List<List<double>> ArrX { get; set; } 
        private static List<List<double>> ArrY  { get; set; }
        private static List<List<double>> ArrZ  { get; set; }

        private static List<string> Legends { get; set; }
        private static List<int> Colours { get; set; }

        private static string _mOptions;

        private static string _xLabel = "x";
        private static string _yLabel = "y";
        private static string _zLabel = "z";

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

            _mOptions = "u 1:2:3 w lp pt 7 lc var ";
            
            VecX = new List<double>();
            VecY = new List<double>();
            
            ArrX = new List<List<double>>();
            ArrY = new List<List<double>>();
            
            Legends = new List<string>();
            Colours = new List<int>();

            _xRange = new double[] {-1, 1};
            _yRange = new double[] {-1, 1};
            
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

        public static void AddPointXY(double x, double y)
        {
            
            if (ArrX.Count != 0)
            {
                ArrX.First().Add(x);
                ArrY.First().Add(y);
            }
            else
            {
                ArrX.Add(new List<double>());
                ArrX.First().Add(x);
                
                ArrY.Add(new List<double>());
                ArrY.First().Add(y);
            }
        }
        
        public static void AddDatasetXY(double[] x, double[] y, string title)
        {
            ArrX.Add(x.ToList());
            ArrY.Add(y.ToList());
            
            Legends.Add($"title '{title}'");
        }

        public static void AddLegend(string title)
        {
            Legends.Add($"title '{title}'");
        }

        public static void CleanData()
        {
            ArrX.Clear();
            ArrY.Clear();
            Legends.Clear();
        }


        public static void SetAutoscale(string axes)
        {
            WriteCommand($"set autoscale {axes}{Environment.NewLine}");
        }

        private static void _setRange(Axis axis = Axis.X)
        {
            var minMaxVec = _xRange;
            var axisName = Axis.X.ToString().ToLower();
            switch (axis)
            {
                case Axis.X:
                    break;

                case Axis.Y:
                    axisName = Axis.Y.ToString().ToLower();
                    minMaxVec = _yRange;
                    break;
            }
            
            WriteCommand($"set {axisName}range [{minMaxVec[0]}:{minMaxVec[1]}]{Environment.NewLine}");
        }
        
        public static void SetXRange(double xMin, double xMax)
        {
            _xRange[0] = xMin;
            _xRange[1] = xMax;
            _setRange(axis: Axis.X);
        }

        public static void SetYRange(double yMin, double yMax)
        {
            _yRange[0] = yMin;
            _yRange[1] = yMax;
            _setRange(axis: Axis.Y);
        }

        private static void _setXLabel()
        {
            WriteCommand($"set xlabel '{_xLabel}'");
        }
        public static void SetXLabel(string label)
        {
            _xLabel = label;
            _setXLabel();
        }
        
        private static void _setYLabel()
        {
            WriteCommand($"set ylabel '{_yLabel}'");
        }
        public static void SetYLabel(string label)
        {
            _yLabel = label;
            _setYLabel();
        }
        
        private static void _plotBegin()
        {
            
            if (0 == Legends.Count)
            {
                WriteCommand(_plotInit + _mOptions + "No title" + Environment.NewLine);
                return;
            }
            
            if (1 == Legends.Count)
            {
                WriteCommand(_plotInit + _mOptions + Legends.First() + Environment.NewLine);
                return;
            }
            
            var cmd = _plotInit + _mOptions + Legends.First() + ",";
            for (var k = 1; k < (Legends.Count - 1); k++)
            {
                cmd += $" '-' {_mOptions} {Legends[k]},";
            }
            
            cmd += " '-' " + _mOptions + Legends.Last() + Environment.NewLine;
            WriteCommand(cmd);
            return;
        }

        private static void _plotEnd()
        {
            var cmd = "e" + Environment.NewLine;
            WriteCommand(cmd);
        }

        private static void _plot_xy(double x, double y, int colour)
        {
            WriteCommand($"{x} {y} {colour}{Environment.NewLine}");
        }

        private static void _plotUpdate()
        {
            GnuplotCmd.Flush();
        }


        public static void PlotDatasetXY()
        {
            _plotBegin();
            
            if (ArrX.Count != ArrY.Count)
            {
                throw new Exception("Error in PlotXY: diferent sizes X-Y");
            }

            for (var i = 0; i < ArrX.Count; i++)
            {
                for (var j = 0; j < ArrX[i].Count; j++)
                {
                    _plot_xy(ArrX[i][j], ArrY[i][j], i);
                }
                _plotEnd();
            }
            _plotUpdate();
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