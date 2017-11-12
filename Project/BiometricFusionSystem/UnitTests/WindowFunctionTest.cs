using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SpeechRecognition;
namespace UnitTests
{
    [TestClass]
    public class WindowFunctionTest
    {
        List<short> _samples = new List<short>();
        List<short> _expectedResult = new List<short>();
        [TestInitialize]
        public void PrepareSamples ()
        {
            _samples.Add(-100);
            _samples.Add(-50);
            _samples.Add(0);
            _samples.Add(50);
            _samples.Add(100);
        }

        [TestMethod] 
        public void HammingWindowFunctionCalculatesCorrectValue()
        {

            _expectedResult.Clear();
            _expectedResult.Add((short)-53.836);
            _expectedResult.Add(-50);
            _expectedResult.Add(0);
            _expectedResult.Add((short)3.836);
            _expectedResult.Add((short)53.836);

            Frame f = new Frame(_samples);
            Window HammingWindow = new HammingWindow();
            HammingWindow.ApplyWindow(f);
            for (int i = 0; i<_samples.Count; i++)
            {
                Assert.AreEqual(_expectedResult[i], _samples[i],1);
            }
        }

        [TestMethod]
        public void GaussWindowFunctionCalculatesCorrectValue()
        {

            _expectedResult.Clear();
            _expectedResult.Add((short)-45.7833);
            _expectedResult.Add(-50);
            _expectedResult.Add(0);
            _expectedResult.Add((short)2.19685);
            _expectedResult.Add((short)0.0883826);

            Frame f = new Frame(_samples);
            Window HammingWindow = new GaussWindow();
            HammingWindow.ApplyWindow(f);
            for (int i = 0; i < _samples.Count; i++)
            {
                Assert.AreEqual(_expectedResult[i], _samples[i], 1);
            }
        }
    }
}
