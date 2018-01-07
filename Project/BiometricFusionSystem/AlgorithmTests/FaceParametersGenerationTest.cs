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
        private string _sampleDirectory = @"C:\Users\aleks\Desktop\";
        private string faceFilePath = @"..\..\..\..\..\Documentation\final_face_identification_results.txt";
        private string _validVerificationPath = @"..\..\..\..\..\Documentation\faceValidVerification.txt";
        private string _invalidVerificationPath = @"..\..\..\..\..\Documentation\faceInvalidVerification.txt";

        [TestMethod]
        public void GenerateGaborFilterParemeters()
        {
            string[] datasets = new string[] { "data", "dataDecember" };
            string directory = "faces";
            double[] lambdamin = new double[] { };
            double[] lambdamax = new double[] { };
            int orientationMin = 4;
            int orientationMax = 11;
            double stdxMin = 1;
            double stdxMax = 8;
            double stdyMin = 1;
            double stdyMax = 8;
            double lambdaMin = 1;
            double lambdaMax = 11;
            double bestAverage = 0;
            Result result = new Result();
            for (int orientations = orientationMin; orientations < orientationMax; orientations++)
            {
                for(double stdx = stdxMin; stdx < stdxMax; stdx += 0.5)
                {
                    for (double stdy = stdyMin; stdy < stdyMax; stdy += 0.5)
                    {
                        for(double lambda = lambdaMin; lambda < lambdaMax; lambda++)
                        {
                            double avg = FaceTestToFile(datasets[1], directory, orientations, stdx, stdy, lambda);
                            if(avg > bestAverage)
                            {
                                result = new Result()
                                {
                                    ResStdX = stdx,
                                    ResStdY = stdy,
                                    ResOrientations = orientations,
                                    ResLambda = lambda
                                };
                                bestAverage = avg;
                            }
                        }
                    }
                }

            }
            using (var writer = new StreamWriter(@"..\..\..\..\..\Documentation\bestFaceResult.txt"))
            {
                writer.WriteLine("Lambda: " + result.ResLambda);
                writer.WriteLine("Orientation count: " + result.ResOrientations);
                writer.WriteLine("stdx: " + result.ResStdX);
                writer.WriteLine("stdy: " + result.ResStdY);
                writer.WriteLine("Average: " + bestAverage.ToString("F2"));
            }
        }

        [TestMethod]
        public void ExtractBestFaceResult()
        {
            List<Result> BestParameters = new List<Result>();
            List<string> resultContent = new List<string>();
            double Lambda = 0;
            int orientation = 0;
            double stdX = 0;
            double stdY = 0;
            double average = 0;
            double currentAverage = 0;
            bool read = false;
            foreach (string line in File.ReadLines(@"..\..\..\..\..\Documentation\faceResultExtract.txt"))
            {
                resultContent.Add(line);
                string[] content = line.Split(' ');
                if (content[0] == "RESULT:")
                {
                    currentAverage = Double.Parse(content[2].Substring(1, 2)) + Double.Parse(content[2].Substring(4, 2)) / 100;
                    if (average <= currentAverage)
                    {
                        average = currentAverage;
                        read = true;
                    }
                    else resultContent.Clear();
                }
                if (read)
                {
                    foreach (string resultValue in resultContent)
                    {
                        string[] value = resultValue.Split(' ');
                        if (value[1] == "Lambda")
                        {
                            Lambda = Double.Parse(value[3]);
                        }
                        if (value[1] == "Orientations")
                        {
                            orientation = Int32.Parse(value[3]);
                        }
                        if (value[1] == "StdX")
                        {
                            stdX = Double.Parse(value[3]);
                        }
                        if (value[1] == "StdY")
                        {
                            stdY = Double.Parse(value[3]);
                        }

                    }
                    Result res = new Result();
                    res.ResLambda = Lambda;
                    res.ResOrientations = orientation;
                    res.ResStdX = stdX;
                    res.ResStdY = stdY;
                    res.ResAverage = currentAverage;
                    BestParameters.Add(res);
                    read = false;
                    resultContent.Clear();
                }
            }
            for (int i = BestParameters.Count - 1; i >= 0; i--)
            {
                if (BestParameters[i].ResAverage < average)
                    BestParameters.RemoveAt(i);
            }
            string dataset = "dataDecember";
            string directory = "faces";
            double bestAverage = 0;
            int orientationMin = 5;
            int orientationMax = 11;
            Result bestResult = new Result();
            for (int orientations = orientationMin; orientations < orientationMax; orientations++)
            {
                foreach (var localBest in BestParameters)
                {
                    double avg = FaceTestToFile(dataset, directory, orientations, localBest.ResStdX, localBest.ResStdY, localBest.ResLambda);
                    if (avg > bestAverage)
                    {
                        bestResult = new Result()
                        {
                            ResStdX = localBest.ResStdX,
                            ResStdY = localBest.ResStdY,
                            ResOrientations = orientations,
                            ResLambda = localBest.ResLambda
                        };
                        bestAverage = avg;
                    }
                }
            }
            using (var writer = new StreamWriter(@"..\..\..\..\..\Documentation\bestFaceResult.txt",append: true))
            {
                writer.WriteLine("Lambda: " + bestResult.ResLambda);
                writer.WriteLine("Orientation count: " + bestResult.ResOrientations);
                writer.WriteLine("stdx: " + bestResult.ResStdX);
                writer.WriteLine("stdy: " + bestResult.ResStdY);
                writer.WriteLine("Average: " + bestAverage.ToString("F2"));
            }
        }

        [TestMethod]
        public void FaceIdentification()
        {
            var datasets = new string[] { "data20", "data40", "data60" };
            foreach (var dataset in datasets)
            {
                FaceTestToFile(dataset, "faces", 7, 6, 7.5, 4);
            }

        }

        [TestMethod]
        public double FaceTestToFile(string dataset, string directory, int orientation,
            double stdx, double stdy, double lambda)
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
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.Lambda), extractor.Lambda);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.Orientations), extractor.Orientations);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.StdX), extractor.StdX);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.StdY), extractor.StdY);
                    resultFile.WriteLine("Comment: " + comment);
                }
                //resultFile.WriteLine("Input file: {0}, input class: {1}, result class: {2}, {3}", className + fileId, className, result,
                //    result == className ? "SUCCESS" : "FAILURE");
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

        [TestMethod]
        public void FaceValidVerification()
        {
            var datasets = new string[] { "data20", "data40", "data60" };
            foreach (var dataset in datasets)
            {
                VerificationValidAcceptance(dataset, "faces", orientation: 7, stdx: 6, stdy: 7.5, lambda: 4, threshold: 14);
            }
        }
        [TestMethod]
        public void FaceInvalidVerification()
        {
            var datasets = new string[] { "data20", "data40", "data60" };
            foreach (var dataset in datasets)
            {
                VerificationInvalidAcceptance(dataset, "faces", orientation: 7, stdx: 6, stdy: 7.5, lambda: 4, threshold: 14);
            }
        }
        [TestMethod]
        private void VerificationValidAcceptance(string dataset, string directory, int orientation,
            double stdx, double stdy, double lambda, double threshold = -1)
        {
            int finalMoment = 3;
            string testComment = "";
            int testFilesPerPerson = 2;
            int trainFilesPerPerson = 4;
            var MDC = new MinimumDistanceClassifier();
            var extractor = new FaceFeatureExtractor(finalMoment, lambda, stdx, stdy, orientation);
            TrainMDC(MDC, directory, trainFilesPerPerson, extractor, dataset);
            var results = new List<double>();
            foreach (var dir in GetDirs(dataset))
            {
                for (int i = 0; i < testFilesPerPerson; i++)
                {
                    var path = _sampleDirectory + dataset + @"\" + directory + @"\" + dir + @"\" + dir + (i + trainFilesPerPerson + 1) + ".bmp";
                    if (File.Exists(path))
                    {
                        var featureVector = ExtractFeaturesFace(path, extractor);
                        double result = MDC.FindEuclideanDistance(featureVector, MDC.Classes[dir]);
                        results.Add(result);
                    }
                }
            }
            if(threshold == -1)
            {
                results = results.OrderBy(n => n).ToList();
                SaveVerificationResult(_validVerificationPath, results);
            }
            else
            {
                SaveVerificationThresholdedResult(_validVerificationPath, threshold, results, dataset);
            }
        }
        private void VerificationInvalidAcceptance(string dataset, string directory, int orientation,
            double stdx, double stdy, double lambda, double threshold = -1)
        {
            int finalMoment = 3;
            string testComment = "";
            int testFilesPerPerson = 2;
            int trainFilesPerPerson = 4;
            var MDC = new MinimumDistanceClassifier();
            var extractor = new FaceFeatureExtractor(finalMoment, lambda, stdx, stdy, orientation);
            TrainMDC(MDC, directory, trainFilesPerPerson, extractor, dataset);
            var results = new List<double>();
            foreach (var dir in GetDirs(dataset))
            {
                for (int i = 0; i < testFilesPerPerson; i++)
                {
                    var path = _sampleDirectory + dataset + @"\" + directory + @"\" + dir + @"\" + dir + (i + trainFilesPerPerson + 1) + ".bmp";
                    if (File.Exists(path))
                    {
                        var featureVector = ExtractFeaturesFace(path, extractor);
                        foreach(var d in GetDirs(dataset))
                        {
                            if(d != dir)
                            {
                                double result = MDC.FindEuclideanDistance(featureVector, MDC.Classes[d]);
                                results.Add(result);
                            }
                        }
                    }
                }
            }
            if(threshold == -1)
            {
                results = results.OrderBy(n => n).ToList();
                SaveVerificationResult(_invalidVerificationPath, results);
            }
            else
            {
                SaveVerificationThresholdedResult(_invalidVerificationPath, threshold, results, dataset);
            }

        }

        private void SaveVerificationResult(string path, List<double> results)
        {
            using (var resultFile = new StreamWriter(path, true))
            {
                foreach(var result in results)
                {
                    resultFile.WriteLine(result);
                }
            }
        }

        private void SaveVerificationThresholdedResult(string path, double threshold, List<double> results, string dataset)
        {
            using (var resultFile = new StreamWriter(path, true))
            {
                resultFile.WriteLine("THRESHOLD: {0}, DATASET: {1}", threshold, dataset);
                int belowThreshold = results.Count(n => n < threshold);
                double percent = belowThreshold / (double)results.Count * 100;
                resultFile.WriteLine("RESULT: {0} ({1}/{2})", percent.ToString("F2"), belowThreshold, results.Count);
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
            var vector = extractor.GetFeatureVector(file);
            file.Dispose();
            return vector;
        }

        private string[] GetDirs(string dataset)
        {
            var dirs = Directory.GetDirectories(_sampleDirectory + dataset + @"\algorithm");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            return dirs;
        }

        private class Result
        {
            public double ResLambda { get; set; }
            public int ResOrientations { get; set; }
            public double ResStdX { get; set; }
            public double ResStdY { get; set; }
            public double ResAverage { get; set; }
        }
    }
}
