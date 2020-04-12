﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using SharpPlot;

class Program
{
    static void Main(string[] args)
    {
        Gnuplot.Start(filePath: "/usr/local/bin/gnuplot");
        var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
        
        var sinX = x.Select(Math.Sin).ToArray();
        var sincosX = sinX.Select(Math.Cos).ToArray();
        var sincostanX = sincosX.Select(Math.Tan).ToArray();
        
        // Gnuplot Example 1:
        Gnuplot.AddDatasetXY(x, x, "Test 0");
        Gnuplot.AddDatasetXY(x, sinX, "Test 1");
        Gnuplot.AddDatasetXY(x, sincosX, "Test 2");
        Gnuplot.AddDatasetXY(x, sincostanX, "Test 3");
        Gnuplot.Axis.SetXTicks(new List<double>(){-8,-2, -1, 0, 1, 2, 8});
        Gnuplot.Axis.SetYTicks(-2, 0.25, 2);
        Gnuplot.PlotDatasetXY();
        Gnuplot.Wait();

        // Gnuplot Example 2:
        Gnuplot.CleanData();
        Gnuplot.Axis.SetXRange(-8, 8);
        Gnuplot.Axis.SetYRange(-1, 1);
        Gnuplot.SetXLabel("x");
        Gnuplot.SetYLabel("sin(x)");
        Gnuplot.AddDatasetXY(x, sinX, "sin(x)");
        Gnuplot.PlotDatasetXY(); 
        Gnuplot.Wait();
        
        // Evaluate a special function
        Console.WriteLine(SpecialFunctions.Erf(0.5));

        // Solve a random linear equation system with 500 unknowns
        var m = Matrix<double>.Build.Random(500, 500);
        var v = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Random(500);
        var y = m.Solve(v);
        Console.WriteLine(y);
        
    }
}