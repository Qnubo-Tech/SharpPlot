using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SharpPlot.Utils;

[assembly: InternalsVisibleTo("SharpPlot.UnitTest")]
namespace SharpPlot.Canvas.Figure
{
    public class Figure
    {
        #region Attributes
        protected internal IEnumerable<double> ArrX;
        protected internal IEnumerable<double> ArrY;
        protected internal IEnumerable<double> ArrZ1;
        protected internal IEnumerable<double> ArrZ2;
        #endregion
    
        #region Properties

        protected internal virtual PlotType PlotType => PlotType.Plot;
        public FigureProperties Properties { get; protected internal set; } = new FigureProperties();
        protected internal virtual string PlotInit => " '-' ";
        internal string HeaderPlot => _getHeaderPlot();
        internal string Options => _getOptions();
        protected internal virtual List<string> DataPoints => _streamPoints();
        #endregion
    
        #region Constructors

        protected internal Figure()
        {
            _setUp();
        }
        #endregion
    
        #region Methods

        protected virtual void _setUp() { }

        private string _getHeaderPlot()
        {
            return PlotInit + Options + " " + Properties.OptTitle;
        }

        protected virtual string _getOptions()
        {
            return "";
        }
        private void _plotBegin()
        { 
            Gnuplot.WriteCommand(HeaderPlot);
        }

        private List<string> _streamPoints()
        {
            var x = ArrX.ToList();
            var y = ArrY.ToList();
            List<string> commands = new List<string>();
            switch (PlotType)
            {
                case PlotType.Plot:
                    commands = x.Select((t, idx) => $"{t} {y[idx]}").ToList();
                    break;
                
                case PlotType.Splot:
                    var z = ArrZ1.ToList();
                    commands = x.Select((t, idx) => $"{t} {y[idx]} {z[idx]}").ToList();
                    break;
            }

            commands.Add("e" + Environment.NewLine);

            return commands;
        }
    
        private void _plotFromStdin()
        {
            List<string> commands = _streamPoints();

            foreach (var t in commands)
            {
                Gnuplot.WriteCommand(t);
            }
        }
    
        public void Plot()
        {
            _plotBegin();

            _plotFromStdin();
        }

        public void SetSize(double size)
        {
            Properties.Size = size;
        }
        
        public void SetColor(Color color)
        {
            Properties.Color = color;
        }

        public void SetDashType(DashType dashType)
        {
            Properties.DashType = dashType;
        }

        public void SetMarker(Marker marker)
        {
            Properties.Marker = marker;
        }

        public void SetWidth(double width)
        {
            Properties.Width = width;
        }

        public void SetTitle(string title)
        {
            Properties.Title = title;
        }

        #endregion
    }
    
    public class Scatter2D : Figure
    {
        #region Methods
        protected override string _getOptions()
        {
            return $"u 1:2 {Shape.Points} {Properties.OptSize} {Properties.OptMarker} {Properties.OptColor}";
        }
        #endregion

    }
    
    public class Line2D : Figure
    {
        #region Methods
        protected override string _getOptions()
        {
            return $"u 1:2 {Shape.Lines} {Properties.OptWidth} {Properties.OptDashType} {Properties.OptColor}";
        }
        #endregion

    }

    public class FilledCurves : Figure
    {
        #region Properties

        protected internal override List<string> DataPoints => _getDataPoints();

        #endregion

        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.FilledCurve} {Properties.OptWidth} {Properties.OptDashType} {Properties.OptColor}";
        }

        private List<string> _getDataPoints()
        {
            var x = ArrX.ToList();
            var y = ArrY.ToList();
            var z = ArrZ1.ToList();
            var commands = x.Select((t, idx) => $"{t} {y[idx]} {z[idx]}").ToList();
            commands.Add("e" + Environment.NewLine);

            return commands;
        }

        #endregion
    }

    public class LinePoints2D : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2 {Shape.LinesPoints} {Properties.OptWidth} {Properties.OptDashType} " +
                   $"{Properties.OptSize} {Properties.OptMarker} {Properties.OptColor}";
        }

        #endregion
    }

    public class YError : Figure
    {
        #region Properties

        protected internal override List<string> DataPoints => _getDataPoints();

        #endregion
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.YErr} {Properties.OptSize} {Properties.OptMarker} {Properties.OptColor}";
        }
        
        private List<string> _getDataPoints()
        {
            var x = ArrX.ToList();
            var y = ArrY.ToList();
            var z = ArrZ1.ToList();
            var commands = x.Select((t, idx) => $"{t} {y[idx]} {z[idx]}").ToList();
            commands.Add("e" + Environment.NewLine);

            return commands;
        }

        #endregion
    } 

    public class Scatter3D : Figure
    {
        #region Properties
        protected internal override PlotType PlotType => PlotType.Splot;
        #endregion
        
        #region Methods
        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.Points} {Properties.OptSize} {Properties.OptMarker} {Properties.OptColor}";
        }
        #endregion
    }

    public class Line3D : Figure
    {
        #region Properties
        protected internal override PlotType PlotType => PlotType.Splot;
        #endregion
        
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.Lines} {Properties.OptWidth} {Properties.OptDashType} {Properties.OptColor}";
        }

        #endregion
    }
    
    public class LinePoints3D : Figure
    {
        #region Properties
        protected internal override PlotType PlotType => PlotType.Splot;
        #endregion
        
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.LinesPoints} {Properties.OptWidth} {Properties.OptDashType} " +
                   $"{Properties.OptSize} {Properties.OptMarker} {Properties.OptColor}";
        }

        #endregion
    }

    public class Function : Figure
    {
        #region Properties

        protected internal override string PlotInit => "";
        protected internal override List<string> DataPoints => new List<string>();

        #endregion

        #region Methods

        protected override string _getOptions()
        {
            return $" {Properties.Function} {Properties.OptColor}";
        }

        #endregion

        
    }

    public class Impulse : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2 {Shape.Impulses} {Properties.OptWidth} {Properties.OptDashType} {Properties.OptColor}";
        }

        #endregion
    }

    public class Bars : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:({Properties.Width}) {Shape.Boxes} {Properties.OptColor}";
        }
        
        #endregion
    }

    public class Figure1D : Figure {}

    public class Histogram : Figure1D
    {
        #region Properties

        protected internal override List<string> DataPoints => _getHistogramPoints();

        #endregion
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:({Properties.Width}) smooth freq {Shape.Boxes} {Properties.OptColor}";
        }

        private IEnumerable<double> _preparePoints()
        {
            var size = ArrX.Count();
            var bins = Math.Min(Math.Ceiling(Math.Sqrt(size)), 100.0);
            var xmax = ArrX.Max();
            var xmin = ArrX.Min();
            var width = (xmax - xmin) / bins;
            var hist = ArrX.Select(e => width * Math.Floor(e / width) + width / 2.0);

            Properties.Width = 0.9 * width;

            return hist;
        }

        private List<string> _getHistogramPoints()
        {
            var x = _preparePoints();
            
            var commands = x.Select(t => $"{t}").ToList();
            
            commands.Add("e" + Environment.NewLine);
            
            return commands;
        }

        #endregion
    }

    public class Boxplot : Figure1D
    {
        #region Properties

        protected internal override List<string> DataPoints => _getBoxplotPoints();

        #endregion
        #region Methods

        protected override void _setUp()
        {
            Gnuplot.WriteCommand("set style data boxplot");
        }

        protected override string _getOptions()
        {
            return $"u (0.0):1:({Properties.Width}) {Properties.OptMarker} {Properties.OptColor}";
        }

        private List<string> _getBoxplotPoints()
        {
            var commands = ArrX.Select(t => $"{t}").ToList();

            commands.Add("e" + Environment.NewLine);
            
            return commands;
            
        }

        #endregion
    }

    public class Vector : Figure
    {
        #region Properties

        protected internal override List<string> DataPoints => _getVectorPoints();

        #endregion
        
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3:4 {Shape.Vector} {Properties.OptWidth} {Properties.OptColor}";
        }

        private List<string> _getVectorPoints()
        {
            var y = ArrY.ToList();
            var z1 = ArrZ1.ToList();
            var z2 = ArrZ2.ToList();
            var commands = ArrX.Select((t, idx) => $"{t} {y[idx]} {z1[idx]} {z2[idx]}").ToList();

            commands.Add("e" + Environment.NewLine);
            
            return commands;
        }

        #endregion
    }
}