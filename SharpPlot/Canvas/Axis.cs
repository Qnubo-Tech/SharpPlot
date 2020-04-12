using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace SharpPlot.Canvas
{
    public enum Direction
    {
        X=0,
        Y,
        Z
    }
    
    public class Axis
    {
        #region Attributes
        
        private static int _defaultRangeTicks = 5;
        
        private static double[] _xRange;
        private static double[] _yRange;
        private static double[] _zRange;

        private static IEnumerable<double> _xTicks;
        private static IEnumerable<double> _yTicks;
        private static IEnumerable<double> _zTicks;
        
        #endregion

        #region Properties

        public double[] XRange => _xRange;
        public double[] YRange => _yRange;
        public double[] ZRange => _zRange;

        public IEnumerable<double> XTicks => _xTicks;
        public IEnumerable<double> YTicks => _yTicks;
        public IEnumerable<double> ZTicks => _zTicks;

        #endregion

        #region Constructors
        public Axis()
        {
            _xRange = new double[] {-1, 1};
            _yRange = new double[] {-1, 1};
            _zRange = new double[] {-1, 1};

            _xTicks = Generate.LinearRange(-1, 0.5, 1);
            _yTicks = Generate.LinearRange(-1, 0.5, 1);
            _zTicks = Generate.LinearRange(-1, 0.5, 1);
        }
        #endregion

        #region Setters 
        private static void _setRange(double min, double max, Direction direction = Direction.X)
        {
            string axisName = "";
            
            switch (direction)
            {
                case Direction.X:
                    axisName = Direction.X.ToString().ToLower();
                    _xRange[0] = min;
                    _xRange[1] = max;
                    break;

                case Direction.Y:
                    axisName = Direction.Y.ToString().ToLower();
                    _yRange[0] = min;
                    _yRange[1] = max;
                    break;
                
                case Direction.Z:
                    axisName = Direction.Z.ToString().ToLower();
                    _zRange[0] = min;
                    _zRange[1] = max;
                    break;
            }

            Gnuplot.WriteCommand($"set {axisName}range [{min}:{max}]{Environment.NewLine}");
        }
        
        private static void _setTicks(IEnumerable<double> ticksValues, Direction direction = Direction.X)
        {
            string axisName = "";
            switch (direction)
            {
                case Direction.X:
                    _xTicks = ticksValues;
                    axisName = Direction.X.ToString().ToLower();
                    break;

                case Direction.Y:
                    _yTicks = ticksValues;
                    axisName = Direction.Y.ToString().ToLower();
                    break;
                
                case Direction.Z:
                    _zTicks = ticksValues;
                    axisName = Direction.Z.ToString().ToLower();
                    break;
            }

            Gnuplot.WriteCommand($"set {axisName}tics ({string.Join(",", ticksValues)})");
        }
        #endregion
        
        #region Range
        public void SetXRange(double xMin, double xMax)
        {
            _setRange(min:xMin, max:xMax, direction: Direction.X);
            var ticks = Generate.LinearSpaced(_defaultRangeTicks, xMin, xMax);
            _setTicks(ticksValues: ticks, direction: Direction.X);
        }

        public void SetYRange(double yMin, double yMax)
        {
            _setRange(min: yMin, max: yMax, direction: Direction.Y);
            var ticks = Generate.LinearSpaced(_defaultRangeTicks, yMin, yMax);
            _setTicks(ticksValues: ticks, direction: Direction.Y);
        }

        public void SetZRange(double zMin, double zMax)
        {
            _setRange(min: zMin, max: zMax, direction: Direction.Z);
            var ticks = Generate.LinearSpaced(_defaultRangeTicks, zMin, zMax);
            _setTicks(ticksValues: ticks, direction: Direction.Z);
        }

        
        #endregion

        #region Ticks
        public void SetXTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, direction: Direction.X);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }

        public void SetXTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, direction: Direction.X);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }

        public void SetXTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, direction: Direction.X);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }
        
        public void SetYTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, direction: Direction.Y);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        public void SetYTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Y);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        public void SetYTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Y);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        
        public void SetZTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, direction: Direction.Z);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        public void SetZTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Z);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        public void SetZTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Z);
            _setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        #endregion

        
    }
}