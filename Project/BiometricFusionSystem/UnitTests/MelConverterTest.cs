using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
namespace UnitTests
{
    [TestClass]
    public class MelConverterTest
    {
        [TestMethod]
        public void ResultOfMelCalculationFromFrequency()
        {
            double expected = 998.21609437;
            double frequency = 1000;
            Assert.AreEqual(expected, MelConverter.ToMel(frequency), 0.00001);
        }
        [TestMethod]
        public void ResultOfFrequencyCalculationFromMel()
        {
            double expected = 1000;
            double mel = 998.21609437;
            Assert.AreEqual(expected, MelConverter.ToFrequency(mel), 0.00001);

        }
    }
}
