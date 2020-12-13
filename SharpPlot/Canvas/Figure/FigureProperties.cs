using SharpPlot.Utils;

namespace SharpPlot.Canvas.Figure
{
    public class FigureProperties
    {
        #region Attributes
        protected internal double Size = 1;
        protected internal Color Color = Color.Black;
        protected internal DashType DashType = DashType.Solid;
        protected internal Marker Marker = Marker.ColoredCircle;
        protected internal double Width = 1.0;
        protected internal string Title = "";
        #endregion
    }
}