using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SharpPlot.Canvas;

namespace SharpPlot.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }

    public class TestAxisRange
    {
        private AxisRange _axisRangeDefault;
        private AxisRange _axisRangeCompleteCttr;
        
        [SetUp]
        public void SetUp()
        {
            _axisRangeDefault = new AxisRange();
            _axisRangeCompleteCttr = new AxisRange(-5, 5, Direction.Z);
        }

        [Test]
        public void TestDefaultCttr()
        {
            Assert.AreEqual(new double[2] {-1, 1}, _axisRangeDefault.Limits);
        }

        [Test]
        public void TestCompleteCttr()
        {
            Assert.AreEqual(new double[2] {-5, 5}, _axisRangeCompleteCttr.Limits);
        }

        [Test]
        public void TestSetRange()
        {
            var actual = _axisRangeCompleteCttr.SetRange(-10, 10);
            var expected = $"set zrange [-10:10]{Environment.NewLine}";
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void MethodTest()
        {
            var ex = Assert.Throws<ArgumentException>(() => _axisRangeCompleteCttr.SetRange(10, -10));
            var expectedMessage = "10 is not lower than -10";
            Assert.AreEqual(expectedMessage, ex.Message);

        }

    }
}