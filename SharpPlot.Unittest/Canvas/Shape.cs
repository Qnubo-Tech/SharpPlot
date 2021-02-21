using NUnit.Framework;
using SharpPlot.Canvas.Figure;

namespace SharpPlot.UnitTest.Canvas
{
    [TestFixture]
    public class TestShape
    {
        [Test]
        public void TestPoints()
        {
            Assert.AreEqual("with points", Shape.Points);
        }

        [Test]
        public void TestLines()
        {
            Assert.AreEqual("with lines", Shape.Lines);
        }

        [Test]
        public void TestFilledCurves()
        {
            Assert.AreEqual("with filledcurve", Shape.FilledCurve);
        }

        [Test]
        public void TestLinesPoints()
        {
            Assert.AreEqual("with linespoints", Shape.LinesPoints);
        }

        [Test]
        public void TestYErr()
        {
            Assert.AreEqual("with yerr", Shape.YErr);
        }

        [Test]
        public void TestImpulses()
        {
            Assert.AreEqual("with impulses", Shape.Impulses);
        }

        [Test]
        public void TestBoxes()
        {
            Assert.AreEqual("with boxes", Shape.Boxes);
        }

        [Test]
        public void TestVector()
        {
            Assert.AreEqual("with vector", Shape.Vector);
        }
    }
}