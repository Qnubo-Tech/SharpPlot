<p align="center">
  <a href="https://github.com/Qnubo-Tech/"><img src="Docs/logo-pre.png" alt='SharpPlot'></a>
</p>

# SharpPlot

SharpPlot is currently at the very beginning of the development cycle and aims to bring a proper wrapper of 
GNUPlot into **C#**. The fundamental idea behind this project is to bring to .NET core the easiness provided by other 
libraries in the Python's community, e.g. [MatPlotLib](https://matplotlib.org/).

## Requirements

* [Gnuplot](http://www.gnuplot.info/)

## Example

```csharp
static void Main(string[] args)
{
        Gnuplot.Start();
        var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
       
        var sinX = x.Select(Math.Sin).ToArray();
        var sincosX = sinX.Select(Math.Cos).ToArray();
        var sincostanX = sincosX.Select(Math.Tan).ToArray();
        
        // Gnuplot Example 1:
        Gnuplot.AddDatasetXY(x, x, "Test 0");
        Gnuplot.AddDatasetXY(x, sinX, "Test 1");
        Gnuplot.AddDatasetXY(x, sincosX, "Test 2");
        Gnuplot.AddDatasetXY(x, sincostanX, "Test 3");
        Gnuplot.SetXRange(-2,2);
        Gnuplot.PlotDatasetXY();
        Gnuplot.Wait();

        // Gnuplot Example 2:
        Gnuplot.CleanData();
        Gnuplot.SetXRange(-8, 8);
        Gnuplot.SetYRange(-1, 1);
        Gnuplot.SetXLabel("x");
        Gnuplot.SetYLabel("sin(x)");
        Gnuplot.AddDatasetXY(x, sinX, "sin(x)");
        Gnuplot.PlotDatasetXY(); 
        Gnuplot.Wait();      
}
```
Result:
<p align="center">
  <img src="https://github.com/migduroli/SharpPlot/blob/b86e02db97cd9d55543a53d2d7ffbb6af1952eeb/SharpPlot/examples/capture1.PNG?raw=true" alt='gnuplot1'>
</p>

<p align="center">
  <img src="https://github.com/migduroli/SharpPlot/blob/b86e02db97cd9d55543a53d2d7ffbb6af1952eeb/SharpPlot/examples/capture2.PNG?raw=true" alt='gnuplot2'>
</p>

