using System;
using System.Text.RegularExpressions;
using SharpPlot.Utils;

namespace SharpPlot.Canvas
{
    public class Legend
    {
        #region Properties
        public Position Position { get; private set; } = Position.RightTop;
        
        private string LegendPosition => _positionToString(Position);
        
        public string Command => $"set key {LegendPosition}{Environment.NewLine}";

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
            Position = position;
            Gnuplot.WriteCommand(Command);
        }
        #endregion
    }
}