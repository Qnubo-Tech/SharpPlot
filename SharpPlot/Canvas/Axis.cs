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

        private static AxisRange _xRange; 
        private static AxisRange _yRange; 
        private static AxisRange _zRange;
        
        private static AxisRange _xAxisRangeAtt;

        private static IEnumerable<double> _xTicks;
        private static IEnumerable<double> _yTicks;
        private static IEnumerable<double> _zTicks;

        private static string _xlabel;
        private static string _ylabel;
        private static string _zlabel;
        
        #endregion

        #region Properties

        public double[] XRange
        {
            get => _xRange.Limits;
        }

        public IEnumerable<double> XTicks => _xTicks;
        public IEnumerable<double> YTicks => _yTicks;
        public IEnumerable<double> ZTicks => _zTicks;

        public static string Xlabel => _xlabel;
        public static string Ylabel => _ylabel;
        public static string Zlabel => _zlabel;

        #endregion

        #region Constructors
        public Axis()
        {
            _xRange = new AxisRange();
            _yRange = new AxisRange(Direction.Y);
            _zRange = new AxisRange(Direction.Z);

            _xTicks = Generate.LinearRange(-1, 0.5, 1);
            _yTicks = Generate.LinearRange(-1, 0.5, 1);
            _zTicks = Generate.LinearRange(-1, 0.5, 1);

            _xlabel = "";
            _ylabel = "";
            _zlabel = "";
        }
        #endregion

        #region Setters 
        private static string _setTicks(IEnumerable<double> ticksValues, Direction direction = Direction.X)
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

            string command = $"set {axisName}tics ({string.Join(",", ticksValues)})";
            
            return command;
        }

        private static void _addTicks(Dictionary<String, double> labelValues, Direction direction)
        {
            string axisName = "";
            switch (direction)
            {
                case Direction.X:
                    axisName = Direction.X.ToString().ToLower();
                    break;

                case Direction.Y:
                    axisName = Direction.Y.ToString().ToLower();
                    break;
                
                case Direction.Z:
                    axisName = Direction.Z.ToString().ToLower();
                    break;
            }
            
            foreach (var kv in labelValues)
            {
                Gnuplot.WriteCommand($"set {axisName}tics add ('{kv.Key}' {kv.Value})");
            }
        }

        private static void _setLabel(string label, int rotation=0,  Direction direction = Direction.X)
        {
            string axisName = "";
            switch (direction)
            {
                case Direction.X:
                    _xlabel = label;
                    axisName = Direction.X.ToString().ToLower();
                    break;

                case Direction.Y:
                    _ylabel = label;
                    axisName = Direction.Y.ToString().ToLower();
                    break;
                
                case Direction.Z:
                    _zlabel = label;
                    axisName = Direction.Z.ToString().ToLower();
                    break;
            }
            
            Gnuplot.WriteCommand($"set {axisName}label '{label}' rotate by {rotation}");
            
        }
        #endregion
        
        #region Range
        private static void _setRangAndTicksFromRange(double min, double max, AxisRange axisRange)
        {
            var rangeCommand = axisRange.SetRange(min: min, max: max);
            Gnuplot.WriteCommand(rangeCommand);
            
/*            var ticks = Generate.LinearSpaced(_defaultRangeTicks, min, max);
            var ticksCommand = _setTicks(ticksValues: ticks, direction: direction);
            Gnuplot.WriteCommand(ticksCommand);*/
        }
        
        public void SetXRange(double xMin, double xMax)
        {
            _setRangAndTicksFromRange(min: xMin, max: xMax, axisRange: _xRange);
        }

        public void SetYRange(double yMin, double yMax)
        {
            _setRangAndTicksFromRange(min: yMin, max: yMax, axisRange: _yRange);
        }

        public void SetZRange(double zMin, double zMax)
        {
            _setRangAndTicksFromRange(min: zMin, max: zMax, axisRange: _zRange);
        }
        
        #endregion

        #region Ticks
        public void SetXTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, direction: Direction.X);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }

        public void SetXTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, direction: Direction.X);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }

        public void SetXTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, direction: Direction.X);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }
        
        public void SetYTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, direction: Direction.Y);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        public void SetYTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Y);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        public void SetYTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Y);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        
        public void SetZTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, direction: Direction.Z);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        public void SetZTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Z);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        public void SetZTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, direction: Direction.Z);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        #endregion
        
        #region AddTicks

        public void AddTicks(Dictionary<string, double> labelValues, int axis=0)
        {
            Direction direction = (Direction) axis;
            _addTicks(labelValues: labelValues, direction: direction);
        }
        
        #endregion
        
        #region Labels
        public void SetXLabel(string label, int rotation = 0)
        {
            _setLabel(label: label, rotation: rotation, direction: Direction.X);
        }
        public void SetYLabel(string label, int rotation = 90)
        { 
            _setLabel(label, rotation: rotation, direction: Direction.Y);
        }

        public void SetZLabel(string label, int rotation = 90)
        {
            _setLabel(label, rotation: rotation, direction: Direction.Z);
        }
        #endregion

        
    }

    public class AxisRange
    {
        #region Attributes
        private double[] _limits;
        private Direction _direction;
        #endregion
        
        #region Properties

        public double[] Limits
        {
            get => _limits;
            set => _limits = value;
        }

        #endregion

        #region Constructors

        public AxisRange()
        {
            _limits = new double[] {-1, 1};
            _direction = Direction.X;
        }

        public AxisRange(Direction direction)
        {
            _limits = new double[] {-1, 1};
            _direction = direction;
        }

        public AxisRange(double min, double max, Direction direction)
        {
            _limits = new double[] {min, max};
            _direction = direction;
        }
        #endregion
        
        private string _setRange(double min, double max)
        {
            string axisName = _direction.ToString().ToLower();
            _limits[0] = min;
            _limits[1] = max;
            
            string command = $"set {axisName}range [{min}:{max}]{Environment.NewLine}";

            return command;
        }

        public string SetRange(double min, double max)
        {
            if (min >= max)
            {
                throw new System.ArgumentException($"{min} is not lower than {max}");
            }
            return _setRange(min: min, max: max);
        }
        
    }
}