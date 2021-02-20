using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using SharpPlot.Canvas;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

namespace SharpPlot.UnitTest.Canvas
{
    [TestFixture]
    public class TestFigure
    {
        private Figure _figure;
        private Scatter _scatter2D;
        private Scatter _scatter3D;
        private Line _line2D;
        private FilledCurves _filledCurves;
        private LinePoints _linePoints2D;
        private YError _yError;
        private Line _line3D;
        private LinePoints _linePoints3D;
        private Impulse _impulse;
        private Function _function;
        private Bars _bars;
        private Histogram _histogram;
        private Boxplot _boxplot;
        private Vector _vector;
        private List<double> _x = Generate.LinearSpaced(10, 0, 10).ToList();
        private List<double> _y = Generate.LinearSpaced(10, 0, 10).Select(e => e * 2).ToList();
        private List<double> _z = Generate.LinearSpaced(10, 0, 10).Select(e => e * 4).ToList();
        private double[] _array = new double[1000];
        private string _f = "x**2+y**2";
        
        [SetUp]
        public void SetUp()
        {
            _figure = new Figure()
            {
                Data = new DataPoints(x: _x, y: _y) 
            };
            _scatter2D = new Scatter()
            {
                Data = new DataPoints(x: _x, y: _y)
            };
            _scatter3D = new Scatter()
            {
                Data = new DataPoints(x: _x, y: _y, z: _z) 
            };
            _line2D = new Line()
            {
                Data = new DataPoints(x: _x, y: _y)
            };
            _filledCurves = new FilledCurves()
            {
                Data = new DataPoints(x: _x, y: _y, z: _z) 
            };
            _linePoints2D = new LinePoints()
            {
                Data = new DataPoints(x: _x, y: _y)
            };
            _yError = new YError()
            {
                Data = new DataPoints(x: _x, y: _y, z: _z) 
            };
            _line3D = new Line()
            {
                Data = new DataPoints(x: _x, y: _y, z: _z)
            };
            _linePoints3D = new LinePoints()
            {
                Data = new DataPoints(x: _x, y: _y, z: _z) 
            };
            _impulse = new Impulse();
            _function = new Function()
            {
                Properties = {Function = _f}
            };
            _bars = new Bars();
            Normal.Samples(_array, mean: 0, stddev: 1);
            _histogram = new Histogram()
            {
                Data = new DataPoints(x: _array) 
            };
            _boxplot = new Boxplot()
            {
                Data = new DataPoints(x: _array)
            };
            _vector = new Vector()
            {
                Data = new DataPoints(x1: _x, x2: _y, y1: _z, y2: _z) 
            };
            
        }

        [Test]
        public void TestDefaultFigureProperties()
        {
            Assert.AreEqual(1, _figure.Properties.Size);
            Assert.AreEqual(Color.Black, _figure.Properties.Color);
            Assert.AreEqual(Marker.ColoredCircle, _figure.Properties.Marker);
            Assert.AreEqual(1, _figure.Properties.Width);
            Assert.AreEqual(DashType.Solid, _figure.Properties.DashType);
            Assert.AreEqual("", _figure.Properties.Function);
            Assert.AreEqual("", _figure.Properties.Title);
        }

        [Test]
        public void TestProperties()
        {
            var expectedDataPoints = _x.Select((t, idx) => $"{t} {_y[idx]}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual("", _figure.Options);
            Assert.AreEqual(" '-'  title ''",_figure.HeaderPlot);
            Assert.AreEqual( expectedDataPoints, _figure.DataPoints);
        }

        [Test]
        public void TestSetSize()
        {
            var size = 2;
            _figure.SetSize(size: size);
            Assert.AreEqual(size, _figure.Properties.Size);
        }

        [Test]
        public void TestSetColor()
        {
            var color = Color.Blue;
            _figure.SetColor(color: color);
            Assert.AreEqual(color, _figure.Properties.Color);
        }

        [Test]
        public void TestSetMarker()
        {
            var marker = Marker.ColoredTriangle;
            _figure.SetMarker(marker: marker);
            Assert.AreEqual(marker, _figure.Properties.Marker);
        }

        [Test]
        public void TestSetWidth()
        {
            var width = 5;
            _figure.SetWidth(width: width);
            Assert.AreEqual(width, _figure.Properties.Width);
        }

        [Test]
        public void TestSetDahType()
        {
            var dashType = DashType.DashDotted;
            _figure.SetDashType(dashType: dashType);
            Assert.AreEqual(dashType, _figure.Properties.DashType);
        }

        [Test]
        public void TestSetTitle()
        {
            var title = "test";
            _figure.SetTitle(title: title);
            Assert.AreEqual(title, _figure.Properties.Title);
        }

        [Test]
        public void TestScatter2D()
        {
            var expectedOps = $"u {_scatter2D.Data.OptDim} {Shape.Points} {_scatter2D.Properties.OptSize} " +
                              $"{_scatter2D.Properties.OptMarker} {_scatter2D.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _scatter2D.Options);
        }

        [Test]
        public void TestLine2D()
        {
            var expectedOps = $"u {_line2D.Data.OptDim} {Shape.Lines} {_line2D.Properties.OptWidth} " +
                              $"{_line2D.Properties.OptDashType} {_line2D.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _line2D.Options);
        }

        [Test]
        public void TestFilledCurves()
        {
            var expectedOps = $"u 1:2:3 {Shape.FilledCurve} {_filledCurves.Properties.OptWidth} " +
                              $"{_filledCurves.Properties.OptDashType} {_filledCurves.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _filledCurves.Options);
            
            var expectedDataPoints = _x.Select((t, idx) => $"{t} {_y[idx]} {_z[idx]}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _filledCurves.DataPoints);
        }

        [Test]
        public void TestLinePoints2D()
        {
            var expectedOps = $"u {_linePoints2D.Data.OptDim} {Shape.LinesPoints} {_linePoints2D.Properties.OptWidth} " +
                              $"{_linePoints2D.Properties.OptDashType} {_linePoints2D.Properties.OptSize} " +
                              $"{_linePoints2D.Properties.OptMarker} {_linePoints2D.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _linePoints2D.Options);
        }

        [Test]
        public void TestYErr()
        {
            var expectedOps = $"u 1:2:3 {Shape.YErr} {_yError.Properties.OptSize} {_yError.Properties.OptMarker} " +
                              $"{_yError.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _yError.Options);
            
            var expectedDataPoints = _x.Select((t, idx) => $"{t} {_y[idx]} {_z[idx]}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _yError.DataPoints);
        }

        [Test]
        public void TestScatter3D()
        {
            var expectedOps = $"u {_scatter3D.Data.OptDim} {Shape.Points} {_scatter3D.Properties.OptSize} " +
                              $"{_scatter3D.Properties.OptMarker} {_scatter3D.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _scatter3D.Options);
            
            var expectedDataPoints = _x.Select((t, idx) => $"{t} {_y[idx]} {_z[idx]}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _scatter3D.DataPoints);
        }

        [Test]
        public void TestLine3D()
        {
            var expectedOps = $"u {_line3D.Data.OptDim} {Shape.Lines} {_line3D.Properties.OptWidth} " +
                              $"{_line3D.Properties.OptDashType} {_line3D.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _line3D.Options);
        }

        [Test]
        public void TestLinePoints3D()
        {
            var expectedOps = $"u {_line3D.Data.OptDim} {Shape.LinesPoints} {_linePoints3D.Properties.OptWidth} {_linePoints3D.Properties.OptDashType} " +
                              $"{_linePoints3D.Properties.OptSize} {_linePoints3D.Properties.OptMarker} " +
                              $"{_linePoints3D.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _linePoints3D.Options);
        }

        [Test]
        public void TestImpulse()
        {
            var expectedOps = $"u 1:2 {Shape.Impulses} {_impulse.Properties.OptWidth} {_impulse.Properties.OptDashType} " +
                              $"{_impulse.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _impulse.Options);
        }

        [Test]
        public void TestFunction()
        {
            Assert.AreEqual(_f, _function.Properties.Function);
            Assert.AreEqual("", _function.PlotInit);
            Assert.AreEqual(new List<string>(), _function.DataPoints);
            
            var expectedOps = $" {_function.Properties.Function} {_function.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _function.Options);
        }

        [Test]
        public void TestBars()
        {
            var expectedOps = $"u 1:2:({_bars.Properties.Width}) {Shape.Boxes} {_bars.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _bars.Options);
        }

        [Test]
        public void TestHistogram()
        {
            var expectedOps = $"u 1:({_histogram.Properties.Width}) smooth freq {Shape.Boxes} " +
                              $"{_histogram.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _histogram.Options);

            var x = _histogram.Data.Array[0];
            var bins = Math.Min(Math.Ceiling(Math.Sqrt(x.Count())), 100.0);
            var width = (x.Max() - x.Min()) / bins;
            var expectedDataPoints = x.Select(e => $"{width * Math.Floor(e / width) + width / 2.0}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _histogram.DataPoints);
        }

        [Test]
        public void TestBoxplot()
        {
            var expectedOps = $"u (0.0):1:({_boxplot.Properties.Width}) {_boxplot.Properties.OptMarker} " +
                             $"{_boxplot.Properties.OptColor}";
            Assert.AreEqual(expectedOps, _boxplot.Options);
            
            var expectedDataPoints = _histogram.Data.Array[0].Select(t => $"{t}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _boxplot.DataPoints);
        }

        [Test]
        public void TestVector()
        {
            var expectedOps = $"u 1:2:3:4 {Shape.Vector} {_vector.Properties.OptWidth} " +
                              $"{_vector.Properties.OptColor}";
            
            Assert.AreEqual(expectedOps, _vector.Options);
            
            var expectedDataPoints = _x.Select((t, idx) => $"{t} {_y[idx]} {_z[idx]} {_z[idx]}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _vector.DataPoints);
        }
        
    }
}