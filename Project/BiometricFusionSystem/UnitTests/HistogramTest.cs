using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using FaceRecognition;

namespace UnitTests
{
    [TestClass]
    public class HistogramTest
    {
        Bitmap _bmp;
        int _height;
        int _width;
        const int ColorMax = 256;
        int[] _expectedR;
        int[] _expectedG;
        int[] _expectedB;
        int[] _actualR;
        int[] _actualG;
        int[] _actualB;
        [TestInitialize]
        public void init ()
        {
            _height = 2;
            _width = 2;
            _bmp = new Bitmap(_width, _height);
            for (int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    _bmp.SetPixel(i, j, Color.FromArgb(1, 1, 1));
                }
            }
            _expectedR = new int[ColorMax];
            _expectedG = new int[ColorMax];
            _expectedB = new int[ColorMax];
            _actualR = new int[ColorMax];
            _actualG = new int[ColorMax];
            _actualB = new int[ColorMax];
        }
        [TestMethod]
        public void CreationOfHistogram()
        {
            _expectedR[1] = 4;
            _expectedG[1] = 4;
            _expectedB[1] = 4;
            Histogram.GetHistogram(_bmp, _actualR, _actualG, _actualB);
            Assert.AreEqual(_expectedR[1], _actualR[1]);
            Assert.AreEqual(_expectedG[1], _actualG[1]);
            Assert.AreEqual(_expectedB[1], _actualB[1]);
        }
    }
}
