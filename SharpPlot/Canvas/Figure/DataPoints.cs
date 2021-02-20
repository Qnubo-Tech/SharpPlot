using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPlot.Canvas.Figure
{
    public class DataPoints
    {
        protected internal double[][] Array;

        #region Properties
        public int Dim => Array.Length;
        public string OptDim => _getOptDim();
        #endregion


        #region Constructors
        public DataPoints(IEnumerable<double> x)
        {
            Array  = new double[1][];
            Array[0] = x.ToArray();
        }

        public DataPoints(IEnumerable<double> x, IEnumerable<double> y)
        {
            Array = new double[2][];
            Array[0] = x.ToArray();
            Array[1] = y.ToArray();
            _checkCommensurability();
        }

        public DataPoints(IEnumerable<double> x, IEnumerable<double> y, IEnumerable<double> z)
        {
            Array = new double[3][];
            Array[0] = x.ToArray();
            Array[1] = y.ToArray();
            Array[2] = z.ToArray();
            _checkCommensurability();
        }

        public DataPoints(IEnumerable<double> x1, IEnumerable<double> x2, IEnumerable<double> y1, IEnumerable<double> y2)
        {
            Array = new double[4][];
            Array[0] = x1.ToArray();
            Array[1] = x2.ToArray();
            Array[2] = y1.ToArray();
            Array[3] = y2.ToArray();
            _checkCommensurability();
        }
        #endregion

        #region Methods

        private void _checkCommensurability()
        {
            var lengths = Array.Select(e => e.Length).ToList();
            var commensurate = lengths.Aggregate(true, (current, t) => (current && (t == lengths.First())));
            if (!commensurate) {throw new ApplicationException($"[!] Shape: {string.Join(", ", lengths)}");}
        }

        private string _getOptDim()
        {
            var optDim = "1";
            foreach (var dim in Enumerable.Range(2, Dim-1))
            {
                optDim += $":{dim}";
            }

            return optDim;
        }

        internal List<string> StreamPoints()
        {
            List<string> commands = new List<string>();
            switch (Dim)
            {
                case 1:
                    commands = Array[0].Select((e) => $"{e}").ToList();
                    break;
                
                case 2:
                    commands = Array[0].Select((e, idx) => $"{e} {Array[1][idx]}").ToList();
                    break;
                
                case 3:
                    commands = Array[0].Select((e, idx) => $"{e} {Array[1][idx]} {Array[2][idx]}").ToList();
                    break;
                
                case 4:
                    commands = Array[0].Select((e, idx) => $"{e} {Array[1][idx]} {Array[2][idx]} {Array[3][idx]}").ToList();
                    break;
            }

            commands.Add("e" + Environment.NewLine);

            return commands;
        }

        #endregion

    }
}