using System;
using System.Linq;
using SharpPlot;

namespace GettingStarted
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Gnuplot.Start();
            var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
            var sinX = x.Select(Math.Sin).ToArray();
            var dataSet = new DataSet(x, sinX);
            Gnuplot.PlotLine2D(dataSet, "First Example: sin(x)");
            Gnuplot.Show();
            Gnuplot.Wait();
        }
    }
}
