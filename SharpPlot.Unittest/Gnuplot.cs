using System;
using System.Collections.Generic;
using MathNet.Numerics;
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
        public void TestWrongRange()
        {
            var ex = Assert.Throws<ArgumentException>(() => _axisRangeCompleteCttr.SetRange(10, -10));
            var expectedMessage = "10 is not lower than -10";
            Assert.AreEqual(expectedMessage, ex.Message);

        }

    }

    public class TestAxisTicks
    {
        private AxisTicks _axisTicksDefault;
        private AxisTicks _axisTicksCompleteCttr;
        
        [SetUp]
        public void SetUp()
        {
            _axisTicksDefault = new AxisTicks();
            var ticksValues = Generate.LinearRange(-2, 0.5, 2);
            _axisTicksCompleteCttr = new AxisTicks(ticksValues, Direction.Z);
        }
        
        [Test]
        public void TestDefaultCttr()
        {
            var ticksValues = new double[] {-1, -0.5, 0, 0.5, 1};
            Assert.AreEqual(ticksValues, _axisTicksDefault.Values);
        }

        [Test]
        public void TestCompleteCttr()
        {
            var ticksValues = new double[] {-2, -1.5, -1, -0.5, 0, 0.5, 1, 1.5, 2};
            Assert.AreEqual(ticksValues, _axisTicksCompleteCttr.Values);
        }

        [Test]
        public void TestSetTicks()
        {
            var ticksValues = Generate.LinearRange(0, 1, 5);
            var actual = _axisTicksCompleteCttr.SetTicks(ticksValues);
            var expected = "set ztics (0,1,2,3,4,5)";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnsortedTicks()
        {
            var ticksValues = new double[] {5, 4, 0, 2, 1, 3};
            var actual = _axisTicksCompleteCttr.SetTicks(ticksValues);
            var expected = "set ztics (0,1,2,3,4,5)";

            Assert.AreEqual(expected, actual);
            
        }

        [Test]
        public void TestEmptyTicksValues()
        {
            
            var ticksValues = new List<double>();
            var ex = Assert.Throws<ArgumentException>(() => _axisTicksCompleteCttr.SetTicks(ticksValues));
            var expectedMessage = "ticksValues cannot be empty";

            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }

    public class TestAxisLabel
    {
        private AxisLabel _axisLabelDefault;
        private AxisLabel _axisLabelCompleteCttr;

        [SetUp]
        public void SetUp()
        {
            _axisLabelDefault = new AxisLabel();
            _axisLabelCompleteCttr = new AxisLabel(label: "Label", direction: Direction.Z);
        }

        [Test]
        public void TestDefaultCttr()
        {
            Assert.AreEqual("", _axisLabelDefault.Label);
        }

        [Test]
        public void TestCompleteCttr()
        {
            Assert.AreEqual("Label", _axisLabelCompleteCttr.Label);
        }

        [Test]
        public void TestSetLabel()
        {
            var label = "NewLabel";
            var rotation = 45;
            var actual = _axisLabelCompleteCttr.SetLabel(label: label, rotation: rotation);
            var expected = "set zlabel 'NewLabel' rotate by 45";
            
            Assert.AreEqual(expected, actual);
        }
        
    }
}