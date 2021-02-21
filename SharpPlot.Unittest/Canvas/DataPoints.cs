using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using NUnit.Framework;
using SharpPlot.Canvas.Figure;

namespace SharpPlot.UnitTest.Canvas
{
    [TestFixture]
    public class TestDataPoints
    {
        private DataPoints _dataPoints1D;
        private DataPoints _dataPoints2D;
        private DataPoints _dataPoints3D;
        private DataPoints _dataPoints4D;
        private List<double> _x = Generate.LinearSpaced(10, 0, 10).ToList();
        private List<double> _xWrongSize = Generate.LinearSpaced(20, 0, 10).ToList();
        private List<double> _y = Generate.LinearSpaced(10, 0, 10).Select(e => e * 2).ToList();
        private List<double> _z = Generate.LinearSpaced(10, 0, 10).Select(e => e * 4).ToList();
        

        [SetUp]
        public void SetUp()
        {
            _dataPoints1D = new DataPoints(x: _x);
            _dataPoints2D = new DataPoints(x: _x, y: _y);
            _dataPoints3D = new DataPoints(x: _x, y: _y, z: _z);
            _dataPoints4D = new DataPoints(x1: _x, x2: _y, y1: _z, y2: _z);
        }

        [Test]
        public void Test1DConstructor()
        {
            var expectedArray = new double[1][];
            expectedArray[0] = _x.ToArray();
            Assert.AreEqual(expectedArray, _dataPoints1D.Array);
        } 
        
        [Test]
        public void Test2DConstructor()
        {
            var expectedArray = new double[2][];
            expectedArray[0] = _x.ToArray();
            expectedArray[1] = _y.ToArray();
            Assert.AreEqual(expectedArray, _dataPoints2D.Array);
        }
        
        [Test]
        public void Test3DConstructor()
        {
            var expectedArray = new double[3][];
            expectedArray[0] = _x.ToArray();
            expectedArray[1] = _y.ToArray();
            expectedArray[2] = _z.ToArray();
            Assert.AreEqual(expectedArray, _dataPoints3D.Array);
        }
        
        [Test]
        public void Test4DConstructor()
        {
            var expectedArray = new double[4][];
            expectedArray[0] = _x.ToArray();
            expectedArray[1] = _y.ToArray();
            expectedArray[2] = _z.ToArray();
            expectedArray[3] = _z.ToArray();
            Assert.AreEqual(expectedArray, _dataPoints4D.Array);
        } 

        [Test]
        public void TestDimProperty()
        {
            Assert.AreEqual(1, _dataPoints1D.Dim);
            Assert.AreEqual(2, _dataPoints2D.Dim);
            Assert.AreEqual(3, _dataPoints3D.Dim);
            Assert.AreEqual(4, _dataPoints4D.Dim);
        }
        
        [Test]
        public void TestOptDimProperty()
        {
            Assert.AreEqual("1", _dataPoints1D.OptDim);
            Assert.AreEqual("1:2", _dataPoints2D.OptDim);
            Assert.AreEqual("1:2:3", _dataPoints3D.OptDim);
            Assert.AreEqual("1:2:3:4", _dataPoints4D.OptDim);
        }

        [Test]
        public void Test1DStreamPoints()
        {
            List<string> expectedStream = new List<string>();
            expectedStream = _x.Select((e) => $"{e}").ToList();
            expectedStream.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedStream, _dataPoints1D.StreamPoints());
        }
        
        [Test]
        public void Test2DStreamPoints()
        {
            List<string> expectedStream = new List<string>();
            expectedStream = _x.Select((e, idx) => $"{e} {_y[idx]}").ToList();
            expectedStream.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedStream, _dataPoints2D.StreamPoints());
        }
        
        [Test]
        public void Test3DStreamPoints()
        {
            List<string> expectedStream = new List<string>();
            expectedStream = _x.Select((e, idx) => $"{e} {_y[idx]} {_z[idx]}").ToList();
            expectedStream.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedStream, _dataPoints3D.StreamPoints());
        }
        
        [Test]
        public void Test4DStreamPoints()
        {
            List<string> expectedStream = new List<string>();
            expectedStream = _x.Select((e, idx) => $"{e} {_y[idx]} {_z[idx]} {_z[idx]}").ToList();
            expectedStream.Add("e" + Environment.NewLine);
            Assert.AreEqual(expectedStream, _dataPoints4D.StreamPoints());
        }

        [Test]
        public void TestCheckCommensurability()
        {
            Assert.Throws<ApplicationException>(() => new DataPoints(x: _x, y: _xWrongSize));
        }


    }
}