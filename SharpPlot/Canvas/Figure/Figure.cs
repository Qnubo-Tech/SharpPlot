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

        protected readonly FigureProperties Properties = new FigureProperties();

        protected List<double> ArrX;
        protected List<double> ArrY;

        #endregion
    
        #region Properties
        public string HeaderPlot => _getHeaderPlot();
        public string Options => _getOptions();
        public List<string> DataPoints => _streamPoints(); 
        #endregion
    
        #region Constructors

        public Figure(){}
    
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
            var commands = new List<string>();
            for (int Idx = 0; Idx < ArrX.Count; Idx++)
            {
                commands.Add($"{ArrX[Idx]} {ArrY[Idx]}");
            }
        
            commands.Add("e" + Environment.NewLine);

            return commands;
        }
    
        private void _plotFromStdin()
        {
            List<string> commands = _streamPoints();
        
            for (int i = 0; i < commands.Count; i++)
            {
                Gnuplot.WriteCommand(commands[i]);
            }
        }
    
        public void Plot()
        {
            _plotBegin();

            _plotFromStdin();
        }

        public void SetColor(Color color)
        {
            Properties.Color = color;
        }

        public void SetTitle(string title)
        {
            Properties.Title = title;
        }

        #endregion
    }
    
    public class Scatter : Figure
    {
        #region Constructors
        public Scatter(IEnumerable<double> x, IEnumerable<double> y, 
            string title, double size, Marker marker, Color color)
        {
            Properties.Title = title;
            Properties.Size = size;
            Properties.Marker = marker;
            Properties.Color = color;
            ArrX = x.ToList();
            ArrY = y.ToList();
        }
        #endregion

        protected override string _getOptions()
        {
            return $"u 1:2 with points ps {Properties.Size} pt {(int) Properties.Marker} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }
    }
    
    public class Line2D : Figure
    {

        #region Constructor
        public Line2D(IEnumerable<double> x, IEnumerable<double> y, string title, double width, DashType dashType, Color color)
        {
            Properties.Title = title;
            Properties.Width = width;
            Properties.DashType = dashType;
            Properties.Color = color;
            ArrX = x.ToList();
            ArrY = y.ToList();
        }
        #endregion
        
        protected override string _getOptions()
        {
            return $"u 1:2 with lines lw {Properties.Width} dt {(int) Properties.DashType} lc rgb '{Properties.Color.ToString().ToLower()}'";
        }
    }
}