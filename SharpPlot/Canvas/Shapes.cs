using System;
using System.Collections.Generic;
using System.Linq;
using SharpPlot.Utils;

namespace SharpPlot.Canvas
{

    public class Figure
    {
        #region Attributes

        private const string PlotInit = " '-' ";

        protected string Options;

        protected string Title;

        protected List<double> ArrX;
        protected List<double> ArrY;

        #endregion
        
        #region Properties
        public string HeaderPlot => _getHeaderPlot();
        public List<string> DataPoints => _streamPoints(); 
        #endregion
        
        #region Constructors

        public Figure(){}
        
        #endregion
        
        #region Methods

        private string _getHeaderPlot()
        {
            return PlotInit + Options + $" title '{Title}' ";
        } 
        private void _plotBegin()
        {
            var command = PlotInit + Options + $" title '{Title}' ";
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
        #endregion
    }
    
    
    public class Scatter : Figure
    {

        #region Constructors
        public Scatter(IEnumerable<double> x, IEnumerable<double> y, 
            string title, double size, Marker marker, Color color)
        {
            Options = $"u 1:2 with points ps {size} pt {(int) marker} lc rgb '{color.ToString().ToLower()}'";
            ArrX = x.ToList();
            ArrY = y.ToList();
            Title = title;
        }
        #endregion
    }
    
    public class Line2D : Figure
    {

        #region Constructor
        public Line2D(IEnumerable<double> x, IEnumerable<double> y, string title, double width, DashType dashType, Color color)
        {
            Options = $"u 1:2 with lines lw {width} dt {(int) dashType} lc rgb '{color.ToString().ToLower()}'";
            ArrX = x.ToList();
            ArrY = y.ToList();
            Title = title;
        }
        #endregion
    }
}