using System;
using System.Collections.Generic;
using System.Linq;
using SharpPlot;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;
using MathNet.Numerics.Distributions;

class Program
{
    static void Main(string[] args)
    {
        Gnuplot.Start();
        var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
        
        var tanX = x.Select(Math.Tan).ToArray();
        var sinX = x.Select(Math.Sin).ToArray();
        var sincosX = sinX.Select(Math.Cos).ToArray();
        var sincostanX = sincosX.Select(Math.Tan).ToArray();
        var expsinX = sinX.Select((e, idx) => e * Math.Exp(-x[idx] / 4)).ToArray();
        
        var dataSet = new DataSet(x, tanX);
        var (lineDatasetId, lineDataset) = Gnuplot.Plot<Line2D>(ds: dataSet);
        lineDataset.SetTitle(title: "WithDataSets");
        lineDataset.SetWidth(width: 2);
        lineDataset.SetDashType(dashType: DashType.DashDotted);
        lineDataset.SetColor(color: Color.Navy);

        // Gnuplot Example 1:
        var (test0Id, test0) = Gnuplot.Plot<Scatter2D>(x, x, "Test 0", size: 0.7, marker: Marker.ColoredCircle, color: Color.SteelBlue);
        var (test1Id, test1) = Gnuplot.Plot<Line2D>(x, sinX, "Test 1", width: 2, dashType: DashType.DashDoubleDotted, color:Color.Red);
        var (test2Id, test2) = Gnuplot.Plot<Scatter2D>(x, sincosX, "Test 2", size: 1, marker: Marker.BlankTriangle, color: Color.Grey);
        var (test3Id, test3) = Gnuplot.Plot<Line2D>(x, sincostanX, "Test 3", width:0.5, dashType: DashType.Solid, color: Color.Green);

        Gnuplot.Axis.SetXTicks(new List<double>(){-4,-2, -1, 0, 1, 2, 4});
        Gnuplot.Axis.SetYTicks(new List<double>(){-4, -3, -1, -0.75, -0.5, -0.25, 0, 0.25, 0.5, 0.75, 1, 3, 4});
        var xticks = new Dictionary<string, double>() { {"pi", Math.PI}, {"phi", 1.618}, {"e", Math.E}};
        Gnuplot.Axis.AddTicks(labelValues: xticks, axis: 0);
        Gnuplot.Axis.SetXLabel(label: "x-axis", rotation: -20);
        Gnuplot.Axis.SetYLabel(label: "y-axis");
        Gnuplot.Legend.SetPosition(position: Position.LeftTop);
        Gnuplot.Show();
        Gnuplot.Wait();
        
        // Gnuplot Impulse example
        Gnuplot.CleanData();
        var (test4Id, test4) = Gnuplot.Plot<Impulse>(x, expsinX);
        test4.SetColor(Color.Brown);
        var (test5Id, test5) = Gnuplot.Plot<Scatter2D>(x, expsinX);
        test5.SetColor(Color.Black);
        test5.SetSize(1.25);
        test5.SetMarker(Marker.BlankCircle);
        Gnuplot.Show();
        Gnuplot.Wait();

        // Gnuplot Example 3D
        Gnuplot.CleanData();
        Gnuplot.SetPlotType(PlotType.Splot);
        Gnuplot.Axis.SetXRange(-8, 8);
        Gnuplot.Axis.SetYRange(-1, 1);
        Gnuplot.Axis.SetZRange(0, 2);
        Gnuplot.Axis.SetXLabel("x");
        Gnuplot.Axis.SetYLabel("y");
        Gnuplot.Axis.SetZLabel("z");
        Gnuplot.Axis.AddTicks(labelValues: xticks, axis: 0);
        var (id, fig) = Gnuplot.Plot<Line3D>(x, sinX, x.Select(e=> 0.1).ToList(), 
            title: "foo",color: Color.SteelBlue, width: 4, dashType:DashType.DashDotted);
        var (id2, fig2) = Gnuplot.Plot<Scatter3D>(x, sinX, sincostanX);
        fig2.SetColor(Color.Grey);
        fig2.SetTitle("bar");
        Gnuplot.Legend.SetPosition(Position.RightBottom);
        Gnuplot.Show();
        Gnuplot.Wait();
        
        // Gnuplot Example Surface with lines
        Gnuplot.CleanData();
        Gnuplot.SetPlotType(PlotType.Splot);
        Gnuplot.SetIsolineDensiy(30);
        var (id3, fig3) = Gnuplot.Plot<Surface3D>(function: "x**2-y**2");
        var (id4, fig4) = Gnuplot.Plot<Surface3D>(function: "x**2+y**2");
        var (id5, fig5) = Gnuplot.Plot<Line3D>(x: x.Select(e=> 0.0).ToList(), y: x, z: x.Select(e=> -(e*e)).ToList());
        var (id6, fig6) = Gnuplot.Plot<Line3D>(x: x, y: x.Select(e=> 0.0).ToList(), z: x.Select(e=> (e*e)).ToList());
        fig3.SetColor(Color.Purple);
        fig3.SetTitle("x**2-y**2");
        fig4.SetColor(Color.Navy);
        fig4.SetTitle("x**2+y**2");
        fig5.SetWidth(2.5);
        fig6.SetWidth(2.5);
        Gnuplot.Show();
        Gnuplot.Wait();
        
        // Gnuplot Example Histogram
        Gnuplot.CleanData();
        var size = 10000;
        var values = new double[size];
        Normal.Samples(values, 0, 2);
        var (id, fig) = Gnuplot.Plot<Histogram>(values);
        fig.SetColor(Color.SteelBlue);
        fig.SetTitle("Normal distribution");
        Gnuplot.FillSolid(alpha: 0.4);
        Gnuplot.Show();
        Gnuplot.Wait();

        // Gnuplot Example 2:
        Gnuplot.CleanData();
        Gnuplot.Axis.SetXRange(-8, 8);
        Gnuplot.Axis.SetYRange(-1, 1);
        Gnuplot.Axis.SetXLabel("x");
        Gnuplot.Axis.SetYLabel("sin(x)");
        var (sinXId, sinXFig) = Gnuplot.Plot<Line2D>(x, sinX);
        Gnuplot.Show(); 
        Gnuplot.Wait();

        Gnuplot.Exit();

    }
}