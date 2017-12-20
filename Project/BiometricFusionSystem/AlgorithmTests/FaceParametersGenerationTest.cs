using Camera;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaceRecognition;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AlgorithmTests
{
    [TestClass]
    public class FaceParametersGenerationTest
    {
        private string _sampleDirectory = @"C:\Users\Martyna\Desktop\";
        private string faceFilePath = @"..\..\..\..\..\Documentation\faceResult.txt";

        [TestMethod]
        public void GenerateGaborFilterParemeters()
        {
            string[] datasets = new string[] { "data", "dataDecember" };
            string directory = "faces";
            double[] lambdamin = new double[] { };
            double[] lambdamax = new double[] { };
            int orientationMin = 4;
            int orientationMax = 8;
            double stdxMin = 0;
            double stdxMax = 10;
            double bestAverage = 0;
            Result result = new Result();

            using (var writer = new StreamWriter(@"..\..\..\..\..\Documentation\bestFaceResult.txt"))
            {
                writer.WriteLine("Lambda: " + result.ResLambda);
                writer.WriteLine("Orientation count: " + result.ResOrientations);
                writer.WriteLine("stdx: " + result.ResStdX);
                writer.WriteLine("stdy: " + result.ResStdY);
                writer.WriteLine("Average: " + bestAverage.ToString("F2"));
                for (int i = 0; i < result.Averages.Count; i++)
                {
                    writer.WriteLine("Average #{0}: {1}", i + 1, result.Averages[i].ToString("F2"));
                }
            }
        }

        [TestMethod]
        public void FaceTestToFileTest()
        {
            FaceTestToFile("dataDecember", "faces", 9, 4, 3, new double[] { 11, 4 });
        }

        [TestMethod]
        public double FaceTestToFile(string dataset, string directory, int orientation,
            double stdx, double stdy, double[] lambda)
        {
            int finalMoment = 3;
            string testComment = "";
            int testFilesPerPerson = 2;
            int trainFilesPerPerson = 4;
            var MDC = new MinimumDistanceClassifier();
            var extractor = new FaceFeatureExtractor(finalMoment, lambda, stdx, stdy, orientation);
            TrainMDC(MDC, directory, trainFilesPerPerson, extractor, dataset);
            bool firstRun = true;
            int countSuccess = 0;
            int countTotal = 0;
            foreach (var dir in GetDirs(dataset))
            {
                for (int i = 0; i < testFilesPerPerson; i++)
                {
                    var path = _sampleDirectory + dataset + @"\" + directory + @"\" + dir + @"\" + dir + (i + trainFilesPerPerson + 1) + ".bmp";
                    if (File.Exists(path))
                    {
                        var featureVector = ExtractFeaturesFace(path, extractor);
                        var result = MDC.Classify(featureVector);
                        bool success = SaveFaceResult(result, extractor, dir, firstRun,
                            i + trainFilesPerPerson + 1, dataset, testComment);
                        if (success)
                        {
                            countSuccess++;
                        }
                        countTotal++;
                        firstRun = false;
                    }
                }
            }
            return SaveTotalResult(faceFilePath, countSuccess, countTotal);
        }

        private bool SaveFaceResult(string result, FaceFeatureExtractor extractor, string className, bool firstRun, int fileId, string dataset, string comment)
        {
            using (var resultFile = new StreamWriter(faceFilePath, true))
            {
                if (firstRun)
                {
                    resultFile.WriteLine("Date: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
                    resultFile.WriteLine("Used dataset: " + dataset);
                    resultFile.WriteLine("Extractor params: ");
                    resultFile.WriteLine("\t {0} : {1} , {2}", nameof(extractor.Lambda), extractor.Lambda[0], extractor.Lambda[1]);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.Orientations), extractor.Orientations);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.StdX), extractor.StdX);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.StdY), extractor.StdY);
                    resultFile.WriteLine("Comment: " + comment);
                }
                resultFile.WriteLine("Input file: {0}, input class: {1}, result class: {2}, {3}", className + fileId, className, result,
                    result == className ? "SUCCESS" : "FAILURE");
            }
            return result == className;
        }

        private double SaveTotalResult(string resultPath, int success, int total)
        {
            using (var resultFile = new StreamWriter(resultPath, true))
            {
                double percent = success / (double)total * 100;
                resultFile.WriteLine("RESULT: {0}/{1} ({2})", success, total, percent.ToString("F2"));
                return percent;
            }
        }

        private void TrainMDC(MinimumDistanceClassifier MDC, string directory, int trainFilesPerPerson, FaceFeatureExtractor extractor,
            string dataset)
        {
            foreach (var dir in GetDirs(dataset))
            {
                var facevectors = new List<List<double>>();
                for (int i = 0; i < trainFilesPerPerson; i++)
                {
                    if (File.Exists(_sampleDirectory + dataset + @"\" + directory + @"\" + dir + @"\" + dir + (i + 1) + ".bmp"))
                    {
                        var path = _sampleDirectory + dataset + @"\" + directory + @"\" + dir + @"\" + dir + (i + 1) + ".bmp";
                        var featureVector = ExtractFeaturesFace(path, extractor);
                        facevectors.Add(featureVector);
                    }
                }
                MDC.AddToDictionary(facevectors, dir);
            }
        }

        private List<double> ExtractFeaturesFace(string path, FaceFeatureExtractor extractor)
        {
            var file = new Bitmap(path);
            return extractor.GetFeatureVector(file);
        }

        private string[] GetDirs(string dataset)
        {
            var dirs = Directory.GetDirectories(_sampleDirectory + dataset + @"\algorithm");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            return dirs;
        }

        private class Result
        {
            public double[] ResLambda { get; set; }
            public int ResOrientations { get; set; }
            public double ResStdX { get; set; }
            public double ResStdY { get; set; }
            public List<double> Averages { get; set; }
        }
    }
}
