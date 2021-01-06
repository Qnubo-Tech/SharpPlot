namespace SharpPlot.Canvas.Figure
{
    internal static class Shape
    {
        #region Properties
        private const string With = "with ";
        internal static string Points => With + "points";
        internal static string Lines => With + "lines";
        internal static string FilledCurve => With + "filledcurve";
        internal static string LinesPoints => With + "linespoints";
        internal static string YErr => With + "yerr";
        internal static string Impulses => With + "impulses";
        internal static string Boxes => With + "boxes";
        internal static string Vector => With + "vector";
        #endregion

    }
}