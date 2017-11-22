using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System.Collections.Generic;
namespace UnitTests
{
    [TestClass]
    public class FrameMakerTest
    {
        FrameMaker _frameMaker;
        List<short> _stream;
        int _sampleRate;
        float _frameLength;
        float _frameInterval;

        [TestInitialize]
        public void Init () 
        {
            _frameLength = 0.1f;
            _frameInterval = 0.05f;
            _frameMaker = new FrameMaker(_frameLength, _frameInterval);
            _stream = new List<short>();
            for (int i = 0; i < 21000 ; i++)
            {
                _stream.Add(0);
            }
            _sampleRate = 44100;
            // no of samples per frame = 4410
            // step 2205
            // no of frames = int(_stream.count / step);
        }
        [TestMethod]
        public void ReturnsCorrectNumberOfFrames()
        {
            Assert.AreEqual(8, _frameMaker.ToFrames(_stream, _sampleRate).Count);
        }
        [TestMethod]
        public void ReturnsCorrectNumberOfSamplesPerFrame()
        {
            Assert.AreEqual(_frameLength * _sampleRate, _frameMaker.ToFrames(_stream, _sampleRate)[0].Samples.Count);
        }
    }
}
