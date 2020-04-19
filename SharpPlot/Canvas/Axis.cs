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

        private static AxisTicks _xTicks;
        private static AxisTicks _yTicks;
        private static AxisTicks _zTicks;

        private static string _xlabel;
        private static string _ylabel;
        private static string _zlabel;
        
        #endregion

        #region Properties

        public double[] XRange => _xRange.Limits;
        public double[] YRange => _yRange.Limits;
        public double[] ZRange => _zRange.Limits;
        
        public IEnumerable<double> XTicks => _xTicks.Values;
        public IEnumerable<double> YTicks => _yTicks.Values;
        public IEnumerable<double> ZTicks => _zTicks.Values;

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

            _xTicks = new AxisTicks();
            _yTicks = new AxisTicks(Direction.Y);
            _zTicks = new AxisTicks(Direction.Z);

            _xlabel = "";
            _ylabel = "";
            _zlabel = "";
        }
        #endregion

        #region Setters 

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
        private void _setRangeAndTicksFromRange(double min, double max, AxisRange axisRange)
        {
            var rangeCommand = axisRange.SetRange(min: min, max: max);
            Gnuplot.WriteCommand(rangeCommand);
            
/*            var ticks = Generate.LinearSpaced(_defaultRangeTicks, min, max);
            var ticksCommand = _setTicks(ticksValues: ticks, direction: direction);
            Gnuplot.WriteCommand(ticksCommand);*/
        }
        
        public void SetXRange(double xMin, double xMax)
        {
            _setRangeAndTicksFromRange(min: xMin, max: xMax, axisRange: _xRange);
        }

        public void SetYRange(double yMin, double yMax)
        {
            _setRangeAndTicksFromRange(min: yMin, max: yMax, axisRange: _yRange);
        }

        public void SetZRange(double zMin, double zMax)
        {
            _setRangeAndTicksFromRange(min: zMin, max: zMax, axisRange: _zRange);
        }
        
        #endregion

        #region Ticks

        private void _setTicks(IEnumerable<double> ticksValues, AxisTicks axisTicks)
        {
            var ticksCommand = axisTicks.SetTicks(ticksValues);
            Gnuplot.WriteCommand(ticksCommand);
        }
        
        public void SetXTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, axisTicks: _xTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }

        public void SetXTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, axisTicks: _xTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }

        public void SetXTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, axisTicks: _xTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.X);
        }
        
        public void SetYTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, axisTicks: _yTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        public void SetYTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, axisTicks: _yTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        public void SetYTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, axisTicks: _yTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Y);
        }
        
        public void SetZTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, axisTicks: _zTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        public void SetZTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, axisTicks: _zTicks);
            //_setRange(min: ticks.Min(), max: ticks.Max(), direction: Direction.Z);
        }
        public void SetZTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, axisTicks: _zTicks);
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
        public double[] Limits => _limits;
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
            
            var command = $"set {axisName}range [{_limits[0]}:{_limits[1]}]{Environment.NewLine}";

            return command;
        }

        public string SetRange(double min, double max)
        {
            if (min >= max)
            {
                throw new ArgumentException($"{min} is not lower than {max}");
            }
            return _setRange(min: min, max: max);
        }
        
    }

    public class AxisTicks
    {
        #region Attributes
        private IEnumerable<double> _values;
        private Direction _direction;
        #endregion

        #region Properties
        public IEnumerable<double> Values => _values;
        #endregion

        #region Cttors

        public AxisTicks()
        {
           _values = Generate.LinearRange(-1, 0.5, 1);
           _direction = Direction.X;
        }

        public AxisTicks(Direction direction)
        {
            _values = Generate.LinearRange(-1, 0.5, 1);
            _direction = direction;
        }

        public AxisTicks(IEnumerable<double> ticksValues, Direction direction)
        {
            _values = ticksValues;
            _direction = direction;
        }
        #endregion

        private string _setTicks(IEnumerable<double> ticksValues)
        {
            string axisName = _direction.ToString().ToLower();
            _values = ticksValues.OrderBy(e => e);

            var command = $"set {axisName}tics ({string.Join(",", _values)})";

            return command;
        }

        public string SetTicks(IEnumerable<double> ticksValues)
        {
            if (!ticksValues.Any())
            {
                throw new ArgumentException($"ticksValues cannot be empty");
            }
            return _setTicks(ticksValues: ticksValues);
        }
        
    }
}