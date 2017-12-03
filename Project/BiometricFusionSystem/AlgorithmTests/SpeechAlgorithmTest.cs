using Camera;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [TestMethod]
        public void AlgorithmAK1_KZSuccessRate0_75()
        {
            var a1 = GetFeatures("AleksanderKusmierczykA1.wav");
            var a2 = GetFeatures("AleksanderKusmierczykA2.wav");
            var a3 = GetFeatures("AleksanderKusmierczykA3.wav");
            var a4 = GetFeatures("AleksanderKusmierczykA4.wav");

            var k1 = GetFeatures("KornelZabaa1.wav");
            var k2 = GetFeatures("KornelZabaa2.wav");
            var k3 = GetFeatures("KornelZabaa3.wav");
            var k4 = GetFeatures("KornelZabaa4.wav");

            var AKResults = new List<double>();
            var KZResults = new List<double>();

            AKResults.Add(_timeWarping.Compare(a1, a2));
            AKResults.Add(_timeWarping.Compare(a1, a3));
            AKResults.Add(_timeWarping.Compare(a1, a4));
            KZResults.Add(_timeWarping.Compare(a1, k1));
            KZResults.Add(_timeWarping.Compare(a1, k2));
            KZResults.Add(_timeWarping.Compare(a1, k3));
            KZResults.Add(_timeWarping.Compare(a1, k4));

            double sum = 0;
            foreach(var akResult in AKResults)
            {
                foreach(var kzResult in KZResults)
                {
                    if(akResult < kzResult)
                    {
                        sum++;
                    }
                }
            }
            sum /= (double)(AKResults.Count * KZResults.Count);
            Assert.IsTrue(sum > 0.75);
        }
        [TestMethod]
        public void AlgorithmAK2_KZSuccessRate0_75()
        {
            var a2 = GetFeatures("AleksanderKusmierczykA2.wav");
            var a3 = GetFeatures("AleksanderKusmierczykA3.wav");
            var a4 = GetFeatures("AleksanderKusmierczykA4.wav");

            var k1 = GetFeatures("KornelZabaa1.wav");
            var k2 = GetFeatures("KornelZabaa2.wav");
            var k3 = GetFeatures("KornelZabaa3.wav");
            var k4 = GetFeatures("KornelZabaa4.wav");

            var AKResults = new List<double>();
            var KZResults = new List<double>();

            AKResults.Add(_timeWarping.Compare(a2, a3));
            AKResults.Add(_timeWarping.Compare(a2, a4));
            KZResults.Add(_timeWarping.Compare(a2, k1));
            KZResults.Add(_timeWarping.Compare(a2, k2));
            KZResults.Add(_timeWarping.Compare(a2, k3));
            KZResults.Add(_timeWarping.Compare(a2, k4));

            double sum = 0;
            foreach (var akResult in AKResults)
            {
                foreach (var kzResult in KZResults)
                {
                    if (akResult < kzResult)
                    {
                        sum++;
                    }
                }
            }
            sum /= (double)(AKResults.Count * KZResults.Count);
            Assert.IsTrue(sum > 0.75);
        }
        [TestMethod]
        public void AlgorithmKZ1_AKSuccessRate0_75()
        {
            var a1 = GetFeatures("AleksanderKusmierczykA1.wav");
            var a2 = GetFeatures("AleksanderKusmierczykA2.wav");
            var a3 = GetFeatures("AleksanderKusmierczykA3.wav");
            var a4 = GetFeatures("AleksanderKusmierczykA4.wav");

            var k1 = GetFeatures("KornelZabaa1.wav");
            var k2 = GetFeatures("KornelZabaa2.wav");
            var k3 = GetFeatures("KornelZabaa3.wav");
            var k4 = GetFeatures("KornelZabaa4.wav");

            var AKResults = new List<double>();
            var KZResults = new List<double>();

            AKResults.Add(_timeWarping.Compare(k1, a2));
            AKResults.Add(_timeWarping.Compare(k1, a3));
            AKResults.Add(_timeWarping.Compare(k1, a4));
            AKResults.Add(_timeWarping.Compare(k1, a1));
            KZResults.Add(_timeWarping.Compare(k1, k2));
            KZResults.Add(_timeWarping.Compare(k1, k3));
            KZResults.Add(_timeWarping.Compare(k1, k4));

            double sum = 0;
            foreach (var akResult in AKResults)
            {
                foreach (var kzResult in KZResults)
                {
                    if (akResult > kzResult)
                    {
                        sum++;
                    }
                }
            }
            sum /= (double)(AKResults.Count * KZResults.Count);
            Assert.IsTrue(sum > 0.75);
        }
        [TestMethod]
        public void AlgorithmKZ2_AKSuccessRate0_75()
        {
            var a1 = GetFeatures("AleksanderKusmierczykA1.wav");
            var a2 = GetFeatures("AleksanderKusmierczykA2.wav");
            var a3 = GetFeatures("AleksanderKusmierczykA3.wav");
            var a4 = GetFeatures("AleksanderKusmierczykA4.wav");

            var k2 = GetFeatures("KornelZabaa2.wav");
            var k3 = GetFeatures("KornelZabaa3.wav");
            var k4 = GetFeatures("KornelZabaa4.wav");

            var AKResults = new List<double>();
            var KZResults = new List<double>();

            AKResults.Add(_timeWarping.Compare(k2, a2));
            AKResults.Add(_timeWarping.Compare(k2, a3));
            AKResults.Add(_timeWarping.Compare(k2, a4));
            AKResults.Add(_timeWarping.Compare(k2, a1));
            KZResults.Add(_timeWarping.Compare(k2, k3));
            KZResults.Add(_timeWarping.Compare(k2, k4));

            double sum = 0;
            foreach (var akResult in AKResults)
            {
                foreach (var kzResult in KZResults)
                {
                    if (akResult > kzResult)
                    {
                        sum++;
                    }
                }
            }
            sum /= (double)(AKResults.Count * KZResults.Count);
            Assert.IsTrue(sum > 0.75);
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
