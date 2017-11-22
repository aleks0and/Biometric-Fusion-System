using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class PeriodogramTest
    {
        Frame _frame;
        List<double> _expectedResult;

        [TestInitialize]
        public void init()
        {
            List<short> frameSample = new List<short>();
            for (int i = 1; i < 5; i++)
            {
                frameSample.Add((short)i);
            }
            _frame = new Frame(frameSample);
            //results calculated in external calculator 
            _expectedResult = new List<double>();
            _expectedResult.Add(6.25);
            _expectedResult.Add(0.5);
        }
        [TestMethod]
        public void ReturnsCorrectCoefficients()
        {

            List<double> actualResult = Periodogram.GetEstimate(_frame);
            for (int i=0; i<_expectedResult.Count; i++)
            {
                Assert.AreEqual(_expectedResult[i], actualResult[i], 0.0001);
            }
        }
    }
}
