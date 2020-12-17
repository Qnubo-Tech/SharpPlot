﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using SharpPlot;
using SharpPlot.Canvas;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

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
        
        var dataSet = new DataSet(x, tanX);
        var (lineDatasetId, lineDataset) = Gnuplot.Plot<Line2D>(ds: dataSet);
        lineDataset.SetTitle(title: "WithDataSets");
        lineDataset.SetWidth(width: 2);
        lineDataset.SetDashType(dashType: DashType.DashDotted);
        lineDataset.SetColor(color: Color.Navy);

        // Gnuplot Example 1:
        var (test0Id, test0) = Gnuplot.Plot<Scatter>(x, x, "Test 0", size: 0.7, marker: Marker.ColoredCircle, color: Color.SteelBlue);
        var (test1Id, test1) = Gnuplot.Plot<Line2D>(x, sinX, "Test 1", width: 2, dashType: DashType.DashDoubleDotted, color:Color.Red);
        var (test2Id, test2) = Gnuplot.Plot<Scatter>(x, sincosX, "Test 2", size: 1, marker: Marker.BlankTriangle, color: Color.Grey);
        var (test3Id, test3) = Gnuplot.Plot<Line2D>(x, sincostanX, "Test 3", width:0.5, dashType: DashType.Solid, color: Color.Green);

        Gnuplot.Axis.SetXTicks(new List<double>(){-4,-2, -1, 0, 1, 2, 4});
        Gnuplot.Axis.SetYTicks(new List<double>(){-4, -3, -1, -0.75, -0.5, -0.25, 0, 0.25, 0.5, 0.75, 1, 3, 4});
        var xticks = new Dictionary<string, double>() { {"pi", Math.PI}, {"phi", 1.618}, {"e", Math.E}};
        Gnuplot.Axis.AddTicks(labelValues: xticks, axis: 0);
        Gnuplot.Axis.SetXLabel(label: "x-axis", rotation: -20);
        Gnuplot.Axis.SetYLabel(label: "y-axis");
        
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