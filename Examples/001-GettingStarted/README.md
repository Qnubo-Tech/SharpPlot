## 001 - GettingStarted example

This first example is aimed at every beginner to start playing with the library, and it also serves as a template first project to see whether everything has been set-up properly.

To run the example:

1. Go to the folder `Examples/001-GettingStarted`

   ```shell
   (SharpPlot)$ cd Examples/001-GettingStarted
   ```

2. The only thing you need to do now is running the project:

   ```shell
   (001-GettingStarted)$ dotnet run
   Press any key + Enter to continue...
   ```

   And you'll get:

   <img src="Figures/simple-project.png" alt="simple-project" style="zoom:33%;" />

The code that has produced such a nice figure is the following:

```c#
using System;
using System.Linq;
using SharpPlot;
using SharpPlot.Canvas.Figure;

namespace GettingStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            Gnuplot.Start();
            var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
            var sinX = x.Select(Math.Sin).ToArray();
            var dataSet = new DataSet(x, sinX);
            var (id, fig) = Gnuplot.Plot<Line2D>(dataSet, "First Example: sin(x)");
            Gnuplot.Show();
            Gnuplot.Wait();
        }
    }
}
```

There are several things to understand from this example:

* We need to import the library with: `using SharpPlot`
* The initialisation of the `gnuplot` interpreter happens with `Gnuplot.Start()`. This is the default constructor of `SharpPlot.Gnuplot`.
* In case `SharpPlot.Gnuplot` is not able to initialise or gives any error, try the `explicit` constructor `Gnuplot("{gnuplot/Bin/Path}")`, which allows you to specify where the executable of gnuplot is located at in your computer.
* `DataSet` is a collection of data, which must be commensurate, i.e. the length of `x` must be equal to the length of `sinX`
* To represent the data with a line, we use `PlotLine2D`, which accepts the legend of this `DataSet` as second argument.

And this is all for the time being. Congratulations, and see you in the next Example &#128079;