using Camera;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmTests
{
    [TestClass]
    public class SpeechAlgorithmTest
    {
        private string _sampleDirPath = @"../../Samples/";
        private FrameMaker _frameMaker;
        private SpeechFeatureExtractor _extractor;
        private DynamicTimeWarping _timeWarping;
        [TestInitialize]
        public void Init()
        {
            _frameMaker = new FrameMaker(frameLength: 0.05f, frameInterval: 0.025f);
            _extractor = new SpeechFeatureExtractor(window: new HammingWindow(), filterbanksCount: 10);
            _timeWarping = new DynamicTimeWarping(threshold: 0.25);
        }
        [TestMethod]
        public void A1A2IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak2.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A2A1IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak2.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A1A3IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak3.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A3A1IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak3.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A1A4IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak4.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A4A1IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak4.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A2A3IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak2.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak3.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A3A2IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak3.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak2.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A2A4IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak2.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak4.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A4A2IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak4.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak2.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A3A4IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak3.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak4.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        [TestMethod]
        public void A4A3IdentificationTest()
        {
            var templateAleks = GetFeatures("aleks_kurczak4.wav");
            var templateMartyna = GetFeatures("martyna_kurczak.wav");
            var templateKornel = GetFeatures("kornel_kurczak.wav");
            var inputAleks = GetFeatures("aleks_kurczak3.wav");

            var aleksVsAleks = _timeWarping.Compare(inputAleks, templateAleks);
            var martynaVsAleks = _timeWarping.Compare(inputAleks, templateMartyna);
            var kornelVsAleks = _timeWarping.Compare(inputAleks, templateKornel);

            Assert.IsTrue(aleksVsAleks < kornelVsAleks);
            Assert.IsTrue(kornelVsAleks < martynaVsAleks);
        }
        private List<double> GetFeatures(string filePath)
        {
            string path = _sampleDirPath + filePath;
            var file = WavReader.Read(path);
            var sampleRate = (int)file.Header.sampleRate;
            var frames = _frameMaker.ToFrames(file.LeftChannel, sampleRate);

            return _extractor.GetFeatures(frames, sampleRate);
        }
    }

}
