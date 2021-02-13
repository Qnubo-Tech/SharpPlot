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
        protected internal DataPoints Data;
        #endregion
    
        #region Properties

        protected internal virtual PlotType PlotType => PlotType.Plot;
        public FigureProperties Properties { get; protected internal set; } = new FigureProperties();
        protected internal virtual string PlotInit => " '-' ";
        internal string HeaderPlot => _getHeaderPlot();
        internal string Options => _getOptions();
        protected internal virtual List<string> DataPoints => Data.StreamPoints();
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
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.FilledCurve} {Properties.OptWidth} {Properties.OptDashType} {Properties.OptColor}";
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
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3 {Shape.YErr} {Properties.OptSize} {Properties.OptMarker} {Properties.OptColor}";
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
            var arrx = Data.Array[0];
            var size = arrx.Count();
            var bins = Math.Min(Math.Ceiling(Math.Sqrt(size)), 100.0);
            var xmax = arrx.Max();
            var xmin = arrx.Min();
            var width = (xmax - xmin) / bins;
            var hist = arrx.Select(e => width * Math.Floor(e / width) + width / 2.0);

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
        #region Methods

        protected override void _setUp()
        {
            Gnuplot.WriteCommand("set style data boxplot");
        }

        protected override string _getOptions()
        {
            return $"u (0.0):1:({Properties.Width}) {Properties.OptMarker} {Properties.OptColor}";
        }

        #endregion
    }

    public class Vector : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:3:4 {Shape.Vector} {Properties.OptWidth} {Properties.OptColor}";
        }

        #endregion
    }
}