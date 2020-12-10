using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using SharpPlot;
using SharpPlot.Canvas;
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
        var whitDataSetsId = Gnuplot.PlotLine2D(dataSet, "WithDataSets", Color.Navy);

        // Gnuplot Example 1:
        var test0 = Gnuplot.PlotScatter(x, x, "Test 0");
        var test1 = Gnuplot.PlotLine2D(x, sinX, "Test 1", Color.Red);
        var test2 = Gnuplot.PlotScatter(x, sincosX, "Test 2");
        var test3 =Gnuplot.PlotLine2D(x, sincostanX, "Test 3", Color.Green);
        

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
        var sinXId = Gnuplot.PlotLine2D(x, sinX, "sin(x)", Color.Black);
        Gnuplot.Show(); 
        Gnuplot.Wait();

        Gnuplot.Exit();
        
        // Evaluate a special function
        Console.WriteLine(SpecialFunctions.Erf(0.5));

        // Solve a random linear equation system with 500 unknowns
        var m = Matrix<double>.Build.Random(500, 500);
        var v = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Random(500);
        var y = m.Solve(v);
        Console.WriteLine(y);
        
    }
}