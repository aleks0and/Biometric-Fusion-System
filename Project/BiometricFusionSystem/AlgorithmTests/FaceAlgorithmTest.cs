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
        // we need to change it so we keep the pictures in the DB instead of the project directory
        private string _sampleDirPath = @"../../../../../../Samples/";
        //private string _sampleDirPath = @"../../Samples/";
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
        [TestMethod]
        public void MDCTest()
        {
            List<string> imagePaths = new List<string>();
            List<string> classNames = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                classNames.Add("Martyna");
                imagePaths.Add("MartynaSlawinska" + (i + 1) + ".bmp");
            }
            for (int i = 0; i < 3; i++)
            {
                classNames.Add("Kornel");
                imagePaths.Add("KornelZaba" + (i + 1) + ".bmp");
            }
            var mdc = new MinimumDistanceClassifier();
            LoadBitmaps(imagePaths, classNames, mdc);
            var martyna = GetFeatures("MartynaSlawinska4.bmp");
            var kornel = GetFeatures("KornelZaba4.bmp");
            Assert.AreEqual("Kornel", mdc.Classify(kornel));
            Assert.AreEqual("Martyna", mdc.Classify(martyna));
            //load images paths to listBmp, class names to classNames, and call LoadBitmaps(listBmp, classNames)
        }

        [TestMethod]
        public void ABADFemaleToFemaleSampleVerification()
        {
            TestMDC("AB", "AD");
        }

        [TestMethod]
        public void APBCMaleToMaleSampleVerification()
        {
            TestMDC("AP", "BC");
        }

        [TestMethod]
        public void DCLXMaleToMaleSampleVerification()
        {
            TestMDC("DC", "LX");
        }

        [TestMethod]
        public void MSMS2MaleToFemaleSampleVerification()
        {
            TestMDC("MS", "MS2");
        }

        private List<double> GetFeatures(string filePath)
        {
            string path = _sampleDirPath + filePath;
            var bmp = new Bitmap(path);
            var features = _extractor.GetFeatureVector(bmp);
            bmp.Dispose();
            return features;
        }

        private List<double> GetFeatures(string filePath, string directoryPath)
        {
            string path = directoryPath + filePath;
            var bmp = new Bitmap(path);
            var features = _extractor.GetFeatureVector(bmp);
            bmp.Dispose();
            return features;
        }

        private void TestMDC(string person1, string person2)
        {
            List<string> imagePaths = new List<string>();
            List<string> classNames = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                classNames.Add(person1);
                imagePaths.Add(person1 + "//" + person1 + (i + 1) + ".bmp");
            }
            for (int i = 0; i < 3; i++)
            {
                classNames.Add(person2);
                imagePaths.Add(person2 + "//" + person2 + (i + 1) + ".bmp");
            }
            var mdc = new MinimumDistanceClassifier();
            LoadBitmaps(imagePaths, classNames, mdc);
            var p1 = GetFeatures(person1 + "//" + person1 + "4.bmp");
            var p2 = GetFeatures(person2 + "//" + person2 + "4.bmp");
            Assert.AreEqual(person1, mdc.Classify(p1));
            Assert.AreEqual(person2, mdc.Classify(p2));
        }

        private List<double> GetGaborFeatures(string filePath)
        {
            string path = _sampleDirPath + filePath;
            var bmp = new Bitmap(path);
            var features = _extractor.GetGaborFeatureVector(bmp);
            bmp.Dispose();
            return features;
        }

        private void LoadBitmaps(List<string> listBmp, List<string> classNames, MinimumDistanceClassifier mdc)
        {
            List<List<double>> fvs = new List<List<double>>();
            List<double> fv = new List<double>();
            for(int i = 0; i < listBmp.Count; i++)
            {
                fv = GetFeatures(listBmp[i]);
                fvs.Add(fv);
            }
            List<List<List<double>>> list = new List<List<List<double>>>();
            List<Tuple<int,string>> numberOfBitmapsPerPerson = new List<Tuple<int, string>>();
            string tempName = classNames[0];
            int tempCount = 0;
            for (int i = 0; i < classNames.Count; i++)
            {
                if (tempName!=classNames[i])
                {
                    numberOfBitmapsPerPerson.Add( new Tuple<int, string>(tempCount, tempName));
                    tempName = classNames[i];
                    tempCount = 0;
                }
                tempCount++;
            }
            numberOfBitmapsPerPerson.Add(new Tuple<int, string>(tempCount, tempName));
            int index = 0;
            foreach (var t in numberOfBitmapsPerPerson)
            {
                var fvForOneClass = new List<List<double>>();
                for (int i = 0; i < t.Item1; i++)
                {
                    fvForOneClass.Add(fvs[index]);
                    index++;
                }
                list.Add(fvForOneClass);
            }
            mdc.Train(list, classNames.Distinct().ToList());
        }
    }
}
