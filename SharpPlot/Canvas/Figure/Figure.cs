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
        protected internal IEnumerable<double> ArrZ;
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
        protected internal Figure() {}
        #endregion
    
        #region Methods

        private string _getHeaderPlot()
        {
            return PlotInit + Options + $" title '{Properties.Title}' ";
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
                    var z = ArrZ.ToList();
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
            return $"u 1:2 with points ps {Properties.Size} pt {(int) Properties.Marker} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }
        #endregion

    }
    
    public class Line2D : Figure
    {
        #region Methods
        protected override string _getOptions()
        {
            return $"u 1:2 with lines lw {Properties.Width} dt {(int) Properties.DashType} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }
        #endregion

    }

    public class LinePoints2D : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2 with linespoints lw {Properties.Width} dt {(int) Properties.DashType} " +
                   $"ps {Properties.Size} pt {(int) Properties.Marker} lc rgb '{Properties.Color.ToString().ToLower()}'";
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
            return $"u 1:2:3 with points ps {Properties.Size} pt {(int) Properties.Marker} lc rgb '{Properties.Color.ToString().ToLower()}'";
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
            return $"u 1:2:3 with lines lw {Properties.Width} dt {(int) Properties.DashType} lc rgb '{Properties.Color.ToString().ToLower()}'";
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
            return $"u 1:2:3 with linespoints lw {Properties.Width} dt {(int) Properties.DashType} " +
                   $"ps {Properties.Size} pt {(int) Properties.Marker} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }

        #endregion
    }

    public class Function : Figure
    {
        #region Properties

        protected internal override string PlotInit => "";
        protected internal override List<string> DataPoints => new List<string>();

        #endregion
        protected override string _getOptions()
        {
            return $" {Properties.Function} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }
        
    }

    public class Impulse : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2 with impulses lw {Properties.Width} dt {(int) Properties.DashType} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }

        #endregion
    }

    public class Bars : Figure
    {
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:2:({Properties.Width}) with boxes lc rgb '{Properties.Color.ToString().ToLower()}'";
        }
        
        #endregion
    }

    public class Histogram : Figure
    {
        #region Properties

        protected internal override List<string> DataPoints => _getHistogramPoints();

        #endregion
        #region Methods

        protected override string _getOptions()
        {
            return $"u 1:({Properties.Width}) smooth freq with boxes lc rgb '{Properties.Color.ToString().ToLower()}'";
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
}