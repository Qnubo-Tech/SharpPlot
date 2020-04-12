using System;

namespace SharpPlot.Canvas
{
    public enum Direction
    {
        X=0,
        Y,
        Z
    }
    
    public class Axis
    {
        #region Attributes
        
        private static double[] _xRange;
        private static double[] _yRange;
        private static double[] _zRange;
        
        #endregion

        #region Properties

        public double[] XRange => _xRange;
        public double[] YRange => _yRange;
        public double[] ZRange => _zRange;
        
        #endregion

        #region Constructors
        public Axis()
        {
            _xRange = new double[] {-1, 1};
            _yRange = new double[] {-1, 1};
            _zRange = new double[] {-1, 1};
        }
        #endregion
        
        private static void _setRange(Direction direction = Direction.X)
        {
            var range = _xRange;
            var axisName = Direction.X.ToString().ToLower();
            switch (direction)
            {
                case Direction.X:
                    break;

                case Direction.Y:
                    axisName = Direction.Y.ToString().ToLower();
                    range = _yRange;
                    break;
                
                case Direction.Z:
                    axisName = Direction.Z.ToString().ToLower();
                    range = _zRange;
                    break;
            }
            
            Gnuplot.WriteCommand($"set {axisName}range [{range[0]}:{range[1]}]{Environment.NewLine}");
        }
        
        public void SetXRange(double xMin, double xMax)
        {
            _xRange[0] = xMin;
            _xRange[1] = xMax;
            _setRange(direction: Direction.X);
        }

        public void SetYRange(double yMin, double yMax)
        {
            _yRange[0] = yMin;
            _yRange[1] = yMax;
            _setRange(direction: Direction.Y);
        }

        public void SetZRange(double zMin, double zMax)
        {
            _zRange[0] = zMin;
            _xRange[1] = zMax;
            _setRange(direction: Direction.Z);
        }
        
    }
}