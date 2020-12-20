using System;
using NUnit.Framework;
using SharpPlot.Canvas;
using SharpPlot.Utils;

namespace SharpPlot.UnitTest.Canvas
{
    [TestFixture]
    public class TestLegend
    {
        private Legend _legend;

        [SetUp]
        public void SetUp()
        {
            _legend = new Legend();
        }

        [Test]
        public void TestDefaultCttr()
        {
            Assert.AreEqual(Position.RightTop, _legend.Position);
            Assert.AreEqual($"set key right top{Environment.NewLine}", _legend.Command);
        }

        [Test]
        public void TestSetPosition()
        {
            _legend.SetPosition(Position.Center);
            Assert.AreEqual(Position.Center,_legend.Position);
            Assert.AreEqual($"set key center{Environment.NewLine}", _legend.Command);
        }

    }
}