using NUnit.Framework;
using SharpPlot.Canvas.Figure;
using SharpPlot.Utils;

namespace SharpPlot.UnitTest.Canvas
{
    [TestFixture]
    public class TestFigureProperties
    {
        private FigureProperties _properties;

        [SetUp]
        public void SetUp()
        {
            _properties = new FigureProperties();
        }
        
        [Test]
        public void TestDefaultProperties()
        {
            Assert.AreEqual(1, _properties.Size);
            Assert.AreEqual(Color.Black, _properties.Color);
            Assert.AreEqual(DashType.Solid, _properties.DashType);
            Assert.AreEqual(Marker.ColoredCircle, _properties.Marker);
            Assert.AreEqual(1, _properties.Width);
            Assert.AreEqual("", _properties.Function);
            Assert.AreEqual("", _properties.Title);
        }

        [Test]
        public void TestOptSize()
        {
            Assert.AreEqual($"ps {_properties.Size}", _properties.OptSize);
        }
        
        [Test]
        public void TestOptColor()
        {
            Assert.AreEqual($"lc rgb '{_properties.Color.ToString().ToLower()}'", _properties.OptColor);
        }
        
        [Test]
        public void TestOptDashType()
        {
            Assert.AreEqual($"dt {(int) _properties.DashType}", _properties.OptDashType);
        }
        
        [Test]
        public void TestOptMarker()
        {
            Assert.AreEqual($"pt {(int) _properties.Marker}", _properties.OptMarker);
        }
        
        [Test]
        public void TestOptWidth()
        {
            Assert.AreEqual($"lw {_properties.Width}", _properties.OptWidth);
        }

        [Test]
        public void TestOptTitle()
        {
            Assert.AreEqual($"title '{_properties.Title}'", _properties.OptTitle);
        }
    }
}