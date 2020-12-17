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
        private Scatter _scatter;
        private Line2D _line2D;
        private List<double> _x = Generate.LinearSpaced(10, 0, 10).ToList();
        private List<double> _y = Generate.LinearSpaced(10, 0, 10).Select(e => e * 2).ToList();

        [SetUp]
        public void SetUp()
        {
            _figure = new Figure()
            {
                ArrX = _x, ArrY = _y
            };
            _scatter = new Scatter();
            _line2D = new Line2D();
        }

        [Test]
        public void TestDefaultFigureProperties()
        {
            Assert.AreEqual(1, _figure.Properties.Size);
            Assert.AreEqual(Color.Black, _figure.Properties.Color);
            Assert.AreEqual(Marker.ColoredCircle, _figure.Properties.Marker);
            Assert.AreEqual(1, _figure.Properties.Width);
            Assert.AreEqual(DashType.Solid, _figure.Properties.DashType);
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
        public void TestScatterOptions()
        {
            var expectedOps = $"u 1:2 with points ps {_scatter.Properties.Size} pt {(int) _scatter.Properties.Marker} " +
                              $"lc rgb '{_scatter.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _scatter.Options);
        }

        [Test]
        public void TestLine2DOptions()
        {
            var expectedOps = $"u 1:2 with lines lw {_line2D.Properties.Width} dt {(int) _line2D.Properties.DashType} " +
                              $"lc rgb '{_line2D.Properties.Color.ToString().ToLower()}'";
            Assert.AreEqual(expectedOps, _line2D.Options);
        }
        
    }
}