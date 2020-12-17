using SharpPlot.Utils;

namespace SharpPlot.Canvas.Figure
{
    public class FigureProperties
    {
        
        #region Properties
        public double Size { get; protected internal set; } = 1;
        public Color Color { get; protected internal set; } = Color.Black;
        public DashType DashType { get; protected internal set; } = DashType.Solid;
        public Marker Marker { get; protected internal set; } = Marker.ColoredCircle;
        public double Width { get; protected internal set; } = 1.0;
        public string Title { get; protected internal set; } = "";
        #endregion
    }
}