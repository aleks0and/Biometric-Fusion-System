using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using FaceRecognition;
using System.Drawing;

namespace AlgorithmTests
{
    [TestClass]
    public class FaceAlgorithmTest
    {
        private string _sampleDirPath = @"../../Samples/";
        private DynamicTimeWarping _timeWarping;
        private FaceFeatureExtractor _extractor;
        private int _finalMoment;
        [TestInitialize]
        public void Init()
        {
            _finalMoment = 3;
            _extractor = new FaceFeatureExtractor(_finalMoment);
            _timeWarping = new DynamicTimeWarping(threshold: 0.25);
        }
        [TestMethod]
        public void A1A2B1S1GaborIdentificationTest ()
        {
            var templateAngelina = GetGaborFeatures("Angelina_Sample2.bmp");
            var templateBrad = GetGaborFeatures("Brad_Sample1.bmp");
            var templateScarlett = GetGaborFeatures("Scarlett_Sample1.bmp");
            var inputAngelina = GetGaborFeatures("Angelina_Sample1.bmp");

            var angelinaVsAngelina = _timeWarping.Compare(inputAngelina, templateAngelina);
            var bradVsAngelina = _timeWarping.Compare(inputAngelina, templateBrad);
            var scarlettVsAngelina = _timeWarping.Compare(inputAngelina, templateScarlett);

            Assert.IsTrue(angelinaVsAngelina < scarlettVsAngelina);
            Assert.IsTrue(scarlettVsAngelina < bradVsAngelina);
        }

        private List<double> GetFeatures(string filePath)
        {
            string path = _sampleDirPath + filePath;
            var bmp = new Bitmap(path);
            var features = _extractor.GetFeatureVector(bmp);
            bmp.Dispose();
            return features;
        }
        private List<double> GetGaborFeatures(string filePath)
        {
            string path = _sampleDirPath + filePath;
            var bmp = new Bitmap(path);
            var features = _extractor.GetGaborFeatureVector(bmp);
            bmp.Dispose();
            return features;
        }
    }
}
