using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using NUnit.Framework;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

namespace SharpPlot.UnitTest.Canvas
{
    [TestFixture]
    public class TestFigure
    {
        private Figure _figure;
        private Scatter2D _scatter2D;
        private Scatter3D _scatter3D;
        private Line2D _line2D;
        private LinePoints2D _linePoints2D;
        private Line3D _line3D;
        private LinePoints3D _linePoints3D;
        private Impulse _impulse;
        private Function _function;
        private List<double> _x = Generate.LinearSpaced(10, 0, 10).ToList();
        private List<double> _y = Generate.LinearSpaced(10, 0, 10).Select(e => e * 2).ToList();
        private List<double> _z = Generate.LinearSpaced(10, 0, 10).Select(e => e * 2).ToList();
        private string _f = "x**2+y**2";
        
        [SetUp]
        public void SetUp()
        {
            _figure = new Figure()
            {
                ArrX = _x, ArrY = _y
            };
            _scatter2D = new Scatter2D();
            _scatter3D = new Scatter3D()
            {
                ArrX = _x, ArrY = _y, ArrZ = _z
            };
            _line2D = new Line2D();
            _linePoints2D = new LinePoints2D();
            _line3D = new Line3D();
            _linePoints3D = new LinePoints3D();
            _impulse = new Impulse();
            _function = new Function()
            {
                Properties = {Function = _f}
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
            Assert.AreEqual(" '-'  title '' ",_figure.HeaderPlot);
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
            var expectedOps = $"u 1:2 with points ps {_scatter2D.Properties.Size} pt {(int) _scatter2D.Properties.Marker} " +
                              $"lc rgb '{_scatter2D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _scatter2D.Options);
        }

        [Test]
        public void TestLine2D()
        {
            var expectedOps = $"u 1:2 with lines lw {_line2D.Properties.Width} dt {(int) _line2D.Properties.DashType} " +
                              $"lc rgb '{_line2D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _line2D.Options);
        }

        [Test]
        public void TestLinePoints2D()
        {
            var expectedOps = $"u 1:2 with linespoints lw {_linePoints2D.Properties.Width} dt {(int) _linePoints2D.Properties.DashType} " +
                              $"ps {_linePoints2D.Properties.Size} pt {(int) _linePoints2D.Properties.Marker} " +
                              $"lc rgb '{_linePoints2D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _linePoints2D.Options);
        }

        [Test]
        public void TestScatter3D()
        {
            Assert.AreEqual(PlotType.Splot, _scatter3D.PlotType);
            
            var expectedOps = $"u 1:2:3 with points ps {_scatter3D.Properties.Size} pt {(int) _scatter3D.Properties.Marker} " +
                              $"lc rgb '{_scatter3D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _scatter3D.Options);
            
            var expectedDataPoints = _x.Select((t, idx) => $"{t} {_y[idx]} {_z[idx]}").ToList();
            expectedDataPoints.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedDataPoints, _scatter3D.DataPoints);
        }

        [Test]
        public void TestLine3D()
        {
            Assert.AreEqual(PlotType.Splot, _line3D.PlotType);
            
            var expectedOps = $"u 1:2:3 with lines lw {_line3D.Properties.Width} dt {(int) _line3D.Properties.DashType} " +
                              $"lc rgb '{_line3D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _line3D.Options);
        }

        [Test]
        public void TestLinePoints3D()
        {
            var expectedOps = $"u 1:2:3 with linespoints lw {_linePoints3D.Properties.Width} dt {(int) _linePoints3D.Properties.DashType} " +
                              $"ps {_linePoints3D.Properties.Size} pt {(int) _linePoints3D.Properties.Marker} " +
                              $"lc rgb '{_linePoints3D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _linePoints3D.Options);
        }

        [Test]
        public void TestImpulse()
        {
            var expectedOps = $"u 1:2 with impulses lw {_impulse.Properties.Width} dt {(int) _impulse.Properties.DashType} " +
                              $"lc rgb '{_impulse.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _impulse.Options);
        }

        [Test]
        public void TestFunction()
        {
            Assert.AreEqual(_f, _function.Properties.Function);
            Assert.AreEqual("", _function.PlotInit);
            Assert.AreEqual(new List<string>(), _function.DataPoints);
            
            var expectedOps = $" {_function.Properties.Function} lc rgb '{_function.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _function.Options);
        }
        
    }
}