using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using FaceRecognition;

namespace UnitTests
{
    [TestClass]
    public class GaborFilterTest
    {
        private Bitmap _bmp;
        private int _width = 4;
        private int _height = 4;
        private double _lambda = 9;
        private int _stdX = 2;
        private int _stdY = 1;
        private double _phase = Math.PI / 2;
        //private int _n = 4;
        private int _kernelSize = 3;
        //private GaborFilter _gabor;
        [TestInitialize]
        public void Init()
        {
            _bmp = new Bitmap(_width, _height);
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _bmp.SetPixel(i, j, Color.White);
                }
            }
            _bmp.SetPixel(1, 1, Color.Black);
            _bmp.SetPixel(1, 2, Color.Black);
           
        }
        [TestMethod]
        public void GaborAngle0Phase90Wave5N2()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase, 2, _kernelSize);
            double actual = gf.GetGaborValue(1, 1, 0, 5);
            double expected = -0.272;
            Assert.AreEqual(expected, actual, 0.01);
        }
        [TestMethod]
        public void GaborAngle135Phase90Wave5N2()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase, 2, _kernelSize);
            double actual = gf.GetGaborValue(1, 1, 3, 5);
            double expected = 0.272;
            Assert.AreEqual(expected, actual, 0.01);
        }
        [TestMethod]
        public void GaborAngle0Phase45Wave5N2()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase / 2, 2, _kernelSize);
            double actual = gf.GetGaborValue(1, 1, 0, 5);
            double expected = -0.129;
            Assert.AreEqual(expected, actual, 0.01);
        }
        [TestMethod]
        public void GaborAngle135Phase45Wave5N2()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase / 2, 2, _kernelSize);
            double actual = gf.GetGaborValue(1, 1, 3, 5);
            double expected = 0.255;
            Assert.AreEqual(expected, actual, 0.01);
        }
        [TestMethod]
        public void GaborAngle135Phase90Wave5N3()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase, 3, _kernelSize);
            double actual = gf.GetGaborValue(1, 1, 3, 5);
            double expected = 0.272;
            Assert.AreEqual(expected, actual, 0.01);
        }
        [TestMethod]
        public void GaborMagnitudeMean()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase, 1, _kernelSize);
            var bmp = gf.ApplyFilter(_bmp);
            GaborFilterMagnitudes magnitudes = new GaborFilterMagnitudes(bmp[0]);
            double actual = magnitudes.CalculateMean();
            double expected = 127.5;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GaborMagnitudeStd()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase, 1, _kernelSize);
            var bmp = gf.ApplyFilter(_bmp);
            GaborFilterMagnitudes magnitudes = new GaborFilterMagnitudes(bmp[0]);
            double mean = 127.5;
            double actual = magnitudes.CalculateStd(mean);
            double expected = 147.2;
            Assert.AreEqual(expected, actual, 0.1);
        }
        [TestMethod]
        public void GaborMagnitudeSkewness()
        {
            GaborFilter gf = new GaborFilter(_stdX, _stdY, _lambda, _phase, 1, _kernelSize);
            var bmp = gf.ApplyFilter(_bmp);
            GaborFilterMagnitudes magnitudes = new GaborFilterMagnitudes(bmp[0]);
            double mean = 127.5;
            double std = 147.2;
            double actual = magnitudes.CalculateSkew(mean, std);
            double expected = 0;
            Assert.AreEqual(expected, actual);
        }
    }
}
