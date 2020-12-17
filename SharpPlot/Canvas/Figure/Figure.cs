using System;
using System.Collections.Generic;
using System.Linq;
using SharpPlot.Utils;

namespace SharpPlot.Canvas.Figure
{
    public class Figure
    {
        #region Attributes
        private const string PlotInit = " '-' ";

        protected internal readonly FigureProperties Properties = new FigureProperties();

        protected internal IEnumerable<double> ArrX;
        protected internal IEnumerable<double> ArrY;
        #endregion
    
        #region Properties

        internal string HeaderPlot => _getHeaderPlot();
        private string Options => _getOptions();
        internal List<string> DataPoints => _streamPoints(); 
        #endregion
    
        #region Constructors
        protected Figure() {}
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
            var command = PlotInit + Options + $" title '{Properties.Title}' ";
            Gnuplot.WriteCommand(command);
        }

        private List<string> _streamPoints()
        {
            var x = ArrX.ToList();
            var y = ArrY.ToList();
            var commands = x.Select((t, idx) => $"{t} {y[idx]}").ToList();

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
    
    public class Scatter : Figure
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
}