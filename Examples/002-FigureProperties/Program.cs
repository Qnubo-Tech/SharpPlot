using System;
using System.Linq;
using SharpPlot;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

namespace FigureProperties
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Gnuplot.Start();
            
            var x = Enumerable.Range(-100, 201).Select(z=>z*0.025*Math.PI).ToArray();
            var y1 = x.Select(e => 1.0).ToArray();
            var sinX = x.Select(Math.Sin).ToArray();
            var cosX = x.Select(Math.Cos).ToArray();

            var (yId, yFig) = Gnuplot.Plot<Line>(x, y: y1);
            yFig.SetTitle(title: "y=1");

            var (xId, xFig) = Gnuplot.Plot<Line>(x: x, y: x, title: "y=x",
                color: Color.Red, dashType: DashType.SmallDash, width: 4);
                
            var dataSin = new DataPoints(x, sinX);
            var dataCos = new DataPoints(x, cosX);    
            
            var (sinId, sinFig) = Gnuplot.Plot<Line>(dp: dataSin, title:"y=sin(x)", 
                color: Color.SteelBlue, dashType: DashType.DashDotted, width: 2.0);
            
            var (cosId, cosFig) = Gnuplot.Plot<Scatter>(dp: dataCos);
            cosFig.SetColor(color: Color.Orange);
            cosFig.SetMarker(marker: Marker.BlankCircle);
            cosFig.SetSize(size: 1.5);
            cosFig.SetTitle(title: "y=cos(x)");
            
            Gnuplot.Axis.SetYRange(-1.5, 1.5);
            Gnuplot.Show();
            Gnuplot.Wait();
        }
    }
}