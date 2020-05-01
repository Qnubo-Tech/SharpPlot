using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPlot.Canvas
{
    public class Scatter
    {
        #region Attributes

        private static readonly string PlotInit = Environment.NewLine + "plot '-' ";

        private static readonly string _options = "u 1:2 with points pt 7";

        private static string _title;

        private static List<double> _arrX;
        private static List<double> _arrY;

        #endregion

        #region Constructors
        public Scatter(IEnumerable<double> x, IEnumerable<double> y, string title)
        {
            _arrX = x.ToList();
            _arrY = y.ToList();
            _title = title;
        }
        #endregion

        #region Methods
        private void _plotBegin()
        {
            var command = PlotInit + _options + $"title '{_title}' " + Environment.NewLine;
            Gnuplot.WriteCommand(command);
        }

        private List<string> _generateCommandsFromPoints()
        {
            var commands = new List<string>();
            for (int Idx = 0; Idx < _arrX.Count; Idx++)
            {
                commands.Add($"{_arrX[Idx]} {_arrY[Idx]}");
            }

            return commands;
        }
        
        private void _plotFromStdin()
        {
            List<string> commands = _generateCommandsFromPoints();
            
            for (int i = 0; i < commands.Count; i++)
            {
                Gnuplot.WriteCommand(commands[i]);
            }

            var endCommand = "e" + Environment.NewLine;
            Gnuplot.WriteCommand(endCommand);
        }
        
        public void Plot()
        {
            _plotBegin();

            _plotFromStdin();
        }
        #endregion
        
    }
}