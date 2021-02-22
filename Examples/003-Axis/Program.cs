using System;
using System.Collections.Generic;
using System.Linq;
using SharpPlot;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

namespace Axis
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Gnuplot.Start();
            
            var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
            var sinX = x.Select(Math.Sin).ToArray();
            var data = new DataPoints(x, sinX);
            
            var (id, fig) = Gnuplot.Plot<Line>(data, "sin(x)");
            
            Gnuplot.Axis.SetYRange(-2, 2);
            Gnuplot.Axis.SetXTicks(start:-4, step: 0.5, stop:4);

            Gnuplot.Axis.SetXLabel(label: "time [s]");
            Gnuplot.Axis.SetYLabel(label: "Amplitude [mm]", rotation: 90);

            Gnuplot.Show();
            Gnuplot.Wait();
            
            Gnuplot.CleanData();
            
            var (id2, fig2) = Gnuplot.Plot<Line>(data, "sin(x)");
            
            var additionalXTicks = new Dictionary<string, double>() { {"pi", Math.PI}, {"e", Math.E} };
            var additionalYTicks = new Dictionary<string, double>() { {"-pi", (-1)*Math.PI}, {"-e", (-1)*Math.E} };
            
            Gnuplot.Axis.SetXRange(0, 8);
            Gnuplot.Axis.AddTicks(labelValues: additionalXTicks, axis: 0);
            Gnuplot.Axis.SetYTicks(ticks: new List<double>(){-4,-2, -1, -0.5, -0.25, 0, 0.25, 0.5, 1, 2, 4});
            Gnuplot.Axis.AddTicks(labelValues: additionalYTicks, axis: 1);
            
            Gnuplot.Show();
            Gnuplot.Wait();
            
            Gnuplot.CleanData();
            
            var data3D = new DataPoints(x: x, y: sinX, z: sinX);
            Gnuplot.SetPlotType(PlotType.Splot);
            var (id3, fig3) = Gnuplot.Plot<Line>(data3D);
            Gnuplot.Axis.RemoveXTicks();
            Gnuplot.Axis.SetYTicks(start: -2, stop: 2, num: 11);
            Gnuplot.Axis.SetZRange(-1, 1);
            Gnuplot.Axis.SetZLabel(label: "z", rotation: 45);
            var additionalZTicks = new Dictionary<string, double>() { {"min", -0.75}, {"max", 0.75} };
            Gnuplot.Axis.AddTicks(additionalZTicks, axis: 2);
            
            Gnuplot.Show();
            Gnuplot.Wait();
        }
    }
}