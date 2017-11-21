using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class DynamicTimeWarpingTest
    {
        private DynamicTimeWarping _dynamicTimeWarping;
        [TestInitialize]
        public void Init()
        {
            _dynamicTimeWarping = new DynamicTimeWarping(threshold: 0.25);
        }
        [TestMethod]
        public void AcceptsSameInputs()
        {
            var input = new List<double>() { 1.0, 2.0, 3.0, -1.0 };
            var template = new List<double>(input);

            Assert.IsTrue(_dynamicTimeWarping.Verify(input, template));
        }
        [TestMethod]
        public void RejectsAboveThreshold()
        {
            var input = new List<double>() { 1.0, 2.0, 3.0, -1.0 };
            var template = new List<double>() { -1.0, 7.5, 1.0 };

            Assert.IsFalse(_dynamicTimeWarping.Verify(input, template));
        }
    }
}
