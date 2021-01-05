using SharpPlot.Utils;

namespace SharpPlot.Canvas.Figure
{
    public class FigureProperties
    {
        
        #region Values
        public double Size { get; protected internal set; } = 1;
        public Color Color { get; protected internal set; } = Color.Black;
        public DashType DashType { get; protected internal set; } = DashType.Solid;
        public Marker Marker { get; protected internal set; } = Marker.ColoredCircle;
        public double Width { get; protected internal set; } = 1.0;
        public string Function { get; protected internal set; } = "";
        public string Title { get; protected internal set; } = "";
        #endregion

        #region OptionsCommand
        internal string OptSize => $"ps {Size}";
        internal string OptColor => $"lc rgb '{Color.ToString().ToLower()}'";
        internal string OptDashType => $"dt {(int) DashType}";
        internal string OptMarker => $"pt {(int) Marker}";
        internal string OptWidth => $"lw {Width}";
        internal string OptTitle => $"title '{Title}'";
        #endregion
    }
}