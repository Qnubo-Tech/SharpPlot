using System;
using System.Text.RegularExpressions;
using SharpPlot.Utils;

namespace SharpPlot.Canvas
{
    public class Legend
    {
        #region Attributes

        private Position _position = Position.RightTop;

        #endregion

        #region Properties

        private string LegendPosition => _positionToString(_position);
        private string Command => $"set key {LegendPosition}{Environment.NewLine}";

        #endregion
        #region Constructor

        public Legend() {}
        #endregion
        
        #region Methods
        private string _positionToString(Position position)
        {
            var foo = Regex.Split(position.ToString(), @"(?<!^)(?=[A-Z])");
            return string.Join(" ", foo).ToLower();
        }

        public void SetPosition(Position position)
        {
            _position = position;
            Gnuplot.WriteCommand(Command);
        }
        #endregion
    }
}