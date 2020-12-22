using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace SharpPlot.Canvas
{
    public enum AxisName
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

        private static AxisLabel _xlabel;
        private static AxisLabel _ylabel;
        private static AxisLabel _zlabel;
        
        #endregion

        #region Properties

        public double[] XRange => _xRange.Limits;
        public double[] YRange => _yRange.Limits;
        public double[] ZRange => _zRange.Limits;
        
        public IEnumerable<double> XTicks => _xTicks.Values;
        public IEnumerable<double> YTicks => _yTicks.Values;
        public IEnumerable<double> ZTicks => _zTicks.Values;

        public string Xlabel => _xlabel.Label;
        public string Ylabel => _ylabel.Label;
        public string Zlabel => _zlabel.Label;

        #endregion

        #region Constructors
        public Axis()
        {
            _xRange = new AxisRange();
            _yRange = new AxisRange(AxisName.Y);
            _zRange = new AxisRange(AxisName.Z);

            _xTicks = new AxisTicks();
            _yTicks = new AxisTicks(AxisName.Y);
            _zTicks = new AxisTicks(AxisName.Z);

            _xlabel = new AxisLabel();
            _ylabel = new AxisLabel(AxisName.Y);
            _zlabel = new AxisLabel(AxisName.Z);
        }
        #endregion

        #region SettersAndCommands 
        private void _setRange(double min, double max, AxisRange axisRange)
        {
            var rangeCommand = axisRange.SetRange(min: min, max: max);
            Gnuplot.WriteCommand(rangeCommand);
        }
        
        private void _setTicks(IEnumerable<double> ticksValues, AxisTicks axisTicks)
        {
            var ticksCommand = axisTicks.SetTicks(ticksValues: ticksValues);
            Gnuplot.WriteCommand(ticksCommand);
        }

        private void _removeTicks(AxisTicks axisTicks)
        {
            var removeTickscommand = axisTicks.RemoveTicks();
            Gnuplot.WriteCommand(removeTickscommand);
        }

        private static void _addTicks(Dictionary<String, double> labelValues, AxisTicks axisTicks)
        {
            List<string> commands = axisTicks.AddTicks(labelValues);

            for (int i = 0; i < commands.Count; i++)
            {
                Gnuplot.WriteCommand(commands[i]);
            }
        }

        private static void _setLabel(string label, int rotation, AxisLabel axisLabel)
        {
            var labelCommand = axisLabel.SetLabel(label: label, rotation: rotation);
            Gnuplot.WriteCommand(labelCommand);
        }
        #endregion
        
        #region Range
        public void SetXRange(double xMin, double xMax)
        {
            _setRange(min: xMin, max: xMax, axisRange: _xRange);
            var ticksValues = Generate.LinearSpaced(_defaultRangeTicks, xMin, xMax);
            _setTicks(ticksValues: ticksValues, axisTicks: _xTicks);
        }

        public void SetYRange(double yMin, double yMax)
        {
            _setRange(min: yMin, max: yMax, axisRange: _yRange);
            var ticksValues = Generate.LinearSpaced(_defaultRangeTicks, yMin, yMax);
            _setTicks(ticksValues: ticksValues, axisTicks: _yTicks);
        }

        public void SetZRange(double zMin, double zMax)
        {
            _setRange(min: zMin, max: zMax, axisRange: _zRange);
            var ticksValues = Generate.LinearSpaced(_defaultRangeTicks, zMin, zMax);
            _setTicks(ticksValues: ticksValues, axisTicks: _zTicks);
        }
        #endregion

        #region Ticks
        public void SetXTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, axisTicks: _xTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _xRange);
        }

        public void SetXTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, axisTicks: _xTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _xRange);
        }

        public void SetXTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, axisTicks: _xTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _xRange);
        }
        
        public void SetYTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, axisTicks: _yTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _yRange);
        }
        public void SetYTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, axisTicks: _yTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _yRange);
        }
        public void SetYTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, axisTicks: _yTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _yRange);
        }
        
        public void SetZTicks(IEnumerable<double> ticks)
        {
            _setTicks(ticksValues: ticks, axisTicks: _zTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _zRange);
        }
        public void SetZTicks(double start, double step, double stop)
        {
            var ticks = Generate.LinearRange(start, step, stop);
            _setTicks(ticksValues: ticks, axisTicks: _zTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _zRange);
        }
        public void SetZTicks(double start, double stop, int num)
        {
            var ticks = Generate.LinearSpaced(num, start, stop);
            _setTicks(ticksValues: ticks, axisTicks: _zTicks);
            _setRange(min: ticks.Min(), max: ticks.Max(), axisRange: _zRange);
        }
        
        public void RemoveXTicks()
        {
            _removeTicks(axisTicks: _xTicks);
        }

        public void RemoveYTicks()
        {
            _removeTicks(axisTicks: _yTicks);
        }

        public void RemoveZTicks()
        {
            _removeTicks(axisTicks: _zTicks);
        }
        #endregion
        
        #region AddTicks
        public void AddTicks(Dictionary<string, double> labelValues, int axis=0)
        {
            AxisName axisName = (AxisName) axis;
            AxisTicks axisTicks;
            switch (axisName)
            {
                case AxisName.X:
                    axisTicks = _xTicks;
                    break;
                case AxisName.Y:
                    axisTicks = _yTicks;
                    break;
                case AxisName.Z:
                    axisTicks = _zTicks;
                    break;
                default:
                    throw new ArgumentException($"axis={axis} is not a valid axis number");
            }
            
            _addTicks(labelValues: labelValues, axisTicks: axisTicks);
        }
        #endregion
        
        #region Labels
        public void SetXLabel(string label, int rotation = 0)
        {
            _setLabel(label: label, rotation: rotation, axisLabel: _xlabel);
        }
        public void SetYLabel(string label, int rotation = 90)
        { 
            _setLabel(label, rotation: rotation, axisLabel: _ylabel);
        }

        public void SetZLabel(string label, int rotation = 90)
        {
            _setLabel(label, rotation: rotation, axisLabel: _zlabel);
        }
        #endregion

        
    }

    public class AxisRange
    {
        #region Attributes
        private double[] _limits;
        private AxisName _axisName;
        #endregion
        
        #region Properties
        public double[] Limits => _limits;
        #endregion

        #region Constructors

        public AxisRange()
        {
            _limits = new double[] {-1, 1};
            _axisName = AxisName.X;
        }

        public AxisRange(AxisName axisName)
        {
            _limits = new double[] {-1, 1};
            _axisName = axisName;
        }

        public AxisRange(double min, double max, AxisName axisName)
        {
            _limits = new double[] {min, max};
            _axisName = axisName;
        }
        #endregion
        
        private string _setRange(double min, double max)
        {
            string axisName = _axisName.ToString().ToLower();
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
        private AxisName _axisName;
        #endregion

        #region Properties
        public IEnumerable<double> Values => _values.OrderBy(e => e);
        #endregion

        #region Cttors

        public AxisTicks()
        {
           _values = Generate.LinearRange(-1, 0.5, 1);
           _axisName = AxisName.X;
        }

        public AxisTicks(AxisName axisName)
        {
            _values = Generate.LinearRange(-1, 0.5, 1);
            _axisName = axisName;
        }

        public AxisTicks(IEnumerable<double> ticksValues, AxisName axisName)
        {
            _values = ticksValues;
            _axisName = axisName;
        }
        #endregion

        private string _setTicks(IEnumerable<double> ticksValues)
        {
            string axisName = _axisName.ToString().ToLower();
            _values = ticksValues.OrderBy(e => e);

            var command = $"set {axisName}tics ({string.Join(",", _values)})";

            return command;
        }

        private string _removeTicks()
        {
            string axisName = _axisName.ToString().ToLower();
            var command = $"set {axisName}tics format ''";
            return command;
        }

        private List<string> _addTicks(Dictionary<string, double> labelValues)
        {
            string axisName = _axisName.ToString().ToLower();

            var commands = new List<string>();
            var enumerable = _values.ToList();
            foreach (var kv in labelValues)
            {
                enumerable.Add(kv.Value);
                commands.Add($"set {axisName}tics add ('{kv.Key}' {kv.Value})");
            }
            
            _values = enumerable;

            return commands;
        }

        public string SetTicks(IEnumerable<double> ticksValues)
        {
            if (!ticksValues.Any())
            {
                throw new ArgumentException($"ticksValues cannot be empty");
            }
            return _setTicks(ticksValues: ticksValues);
        }

        public string RemoveTicks()
        {
            return _removeTicks();
        }

        public List<string> AddTicks(Dictionary<string, double> labelValues)
        {
            return _addTicks(labelValues: labelValues);
        }
        
    }

    public class AxisLabel
    {
        #region Attributes
        private string _label;
        private AxisName _axisName;
        private int _rotation;
        #endregion
        
        #region Properties
        public string Label
        {
            get => _label;
            set => _label = value;
        }

        public int Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        #endregion
        
        #region Cttos
        public AxisLabel()
        {
            _label = "";
            _axisName = AxisName.X;
            _rotation = 0;
        }

        public AxisLabel(AxisName axisName)
        {
            _label = "";
            _axisName = axisName;
            _rotation = 0;
        }

        public AxisLabel(string label, AxisName axisName)
        {
            _label = label;
            _axisName = axisName;
            _rotation = 0;
        }
        #endregion

        private string _setLabel(string label, int rotation)
        {
            string axisName = _axisName.ToString().ToLower();
            _label = label;
            _rotation = rotation;

            var command = $"set {axisName}label '{_label}' rotate by {_rotation}";

            return command;
        }

        public string SetLabel(string label, int rotation)
        {
            return _setLabel(label: label, rotation: rotation);
        }

    }
}