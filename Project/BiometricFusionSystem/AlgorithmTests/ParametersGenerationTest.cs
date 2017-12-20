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
    public class ParametersGenerationTest
    {
        private string _sampleDirectory = @"C:\Users\aleks\Desktop\";
        private string speechFilePath = @"..\..\..\..\..\Documentation\speechResult.txt";
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
                writer.WriteLine("Lambda: " + result.CoeffsLeft);
                writer.WriteLine("Orientation count: " + result.FilterBanks);
                writer.WriteLine("stdx: " + result.FrameLength);
                writer.WriteLine("stdy: " + result.Window.GetType().FullName);
                writer.WriteLine("Average: " + bestAverage.ToString("F2"));
                for (int i = 0; i < result.Averages.Count; i++)
                {
                    writer.WriteLine("Average #{0}: {1}", i + 1, result.Averages[i].ToString("F2"));
                }
            }
        }

        [TestMethod]
        public void GenerateSpeechTests()
        {
            string[] datasets = new string[] { "data", "dataDecember" };
            string[] words = new string[] { "algorithm", "close" };
            Window[] windows = new Window[] { new GaussWindow(), new HammingWindow() };
            int filterBankMin = 8;
            int filterBankMax = 30;
            int coeffMin = 8;
            float frameLengthMin = 0.02f;
            float frameLengthMax = 0.1f;
            double bestAverage = 0;
            Result result = new Result();
            for (float frame = frameLengthMin; frame <= frameLengthMax; frame += 0.005f)
            {
                for (int filter = filterBankMin; filter <= filterBankMax; filter += 2)
                {
                    for (int coeff = coeffMin; coeff <= filter; coeff++)
                    {
                        foreach (Window w in windows)
                        {
                            var averages = new List<double>();
                            foreach (var dataset in datasets)
                            {
                                foreach (var word in words)
                                {
                                    averages.Add(SpeechTestToFile(dataset, word, filter, coeff, frame, frame / 2, w));
                                }
                            }
                            double avg = 0;
                            for (int i = 0; i < datasets.Length * words.Length; i++)
                            {
                                avg += averages[i];
                            }
                            avg /= datasets.Length * words.Length;
                            if (avg > bestAverage)
                            {
                                result = new Result()
                                {
                                    Averages = averages,
                                    CoeffsLeft = coeff,
                                    FilterBanks = filter,
                                    FrameLength = frame,
                                    Window = w
                                };
                                bestAverage = avg;
                            }
                        }
                    }
                }
            }
            using (var writer = new StreamWriter(@"..\..\..\..\..\Documentation\bestResult.txt"))
            {
                writer.WriteLine("Coeffs: " + result.CoeffsLeft);
                writer.WriteLine("Filterbanks: " + result.FilterBanks);
                writer.WriteLine("FrameLength: " + result.FrameLength);
                writer.WriteLine("Window type: " + result.Window.GetType().FullName);
                writer.WriteLine("Average: " + bestAverage.ToString("F2"));
                for (int i = 0; i < result.Averages.Count; i++)
                {
                    writer.WriteLine("Average #{0}: {1}", i + 1, result.Averages[i].ToString("F2"));
                }
            }
        }
        [TestMethod]
        public void FaceTestToFileTest ()
        {
            FaceTestToFile("dataDecember", "faces", 4, 2, 1, new double[] { 10, 5 });
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
                for (int i=0; i < testFilesPerPerson; i++)
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
        [TestMethod]
        public double SpeechTestToFile(string dataset, string word, int filterBanks, int coeffs,
            float frameLength, float frameInterval, Window window)
        {
            string testComment = "";
            int testFilesPerPerson = 2;
            int trainFilesPerPerson = 4;
            var dtw = new DynamicTimeWarping(0);
            var frameMaker = new FrameMaker(frameLength, frameInterval);
            var extractor = new SpeechFeatureExtractor(window, filterBanks, coeffs);
            TrainDtw(dtw, word, trainFilesPerPerson, extractor, frameMaker, dataset);
            bool firstRun = true;
            int countSuccess = 0;
            int countTotal = 0;
            foreach (var dir in GetDirs(dataset))
            {
                for (int i = 0; i < testFilesPerPerson; i++)
                {
                    var path = _sampleDirectory + dataset + @"\" + word + @"\" + dir + @"\" + dir + (i + trainFilesPerPerson + 1) + ".wav";
                    if (File.Exists(path))
                    {
                        var featureVector = ExtractFeaturesVoice(path, extractor, frameMaker);
                        var result = dtw.Classify(featureVector);
                        bool success = SaveSpeechResult(result, extractor, frameMaker, dir, word,
                            firstRun, i + trainFilesPerPerson + 1, dataset, testComment);
                        if (success)
                        {
                            countSuccess++;
                        }
                        countTotal++;
                        firstRun = false;
                    }
                }
            }
            return SaveTotalResult(speechFilePath, countSuccess, countTotal);
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
        private bool SaveSpeechResult(string result, SpeechFeatureExtractor extractor, FrameMaker frameMaker, string className,
            string word, bool firstRun, int fileId, string dataset, string comment)
        {
            using (var resultFile = new StreamWriter(speechFilePath, true))
            {
                if (firstRun)
                {
                    resultFile.WriteLine("Date: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
                    resultFile.WriteLine("Used word: " + word + ", dataset: " + dataset);
                    resultFile.WriteLine("Extractor params: ");
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.FilterBanksCount), extractor.FilterBanksCount);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.CoeffsLeft), extractor.CoeffsLeft);
                    resultFile.WriteLine("\t {0} : {1}", nameof(extractor.WindowFunction), extractor.WindowFunction.GetType().FullName);
                    resultFile.WriteLine("FrameMaker params: ");
                    resultFile.WriteLine("\t {0} : {1}", nameof(frameMaker.FrameLength), frameMaker.FrameLength);
                    resultFile.WriteLine("\t {0} : {1}", nameof(frameMaker.FrameInterval), frameMaker.FrameInterval);
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
        private void TrainDtw(DynamicTimeWarping dtw, string word, int trainFilesPerPerson, SpeechFeatureExtractor extractor,
            FrameMaker frameMaker, string dataset)
        {
            foreach (var dir in GetDirs(dataset))
            {
                var speechVectors = new List<List<double>>();
                for (int i = 0; i < trainFilesPerPerson; i++)
                {
                    if (File.Exists(_sampleDirectory + dataset + @"\" + word + @"\" + dir + @"\" + dir + (i + 1) + ".wav"))
                    {
                        var path = _sampleDirectory + dataset + @"\" + word + @"\" + dir + @"\" + dir + (i + 1) + ".wav";
                        var featureVector = ExtractFeaturesVoice(path, extractor, frameMaker);
                        speechVectors.Add(featureVector);
                    }
                }
                dtw.AddToDictionary(speechVectors, dir);
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
                MDC.AddToDictionary(facevectors,dir);
            }
        }
        private List<double> ExtractFeaturesFace(string path, FaceFeatureExtractor extractor)
        {
            var file = new Bitmap(path);
            return extractor.GetFeatureVector(file);
        }

        private List<double> ExtractFeaturesVoice(string path, SpeechFeatureExtractor extractor, FrameMaker frameMaker)
        {
            var file = WavReader.Read(path);
            var sampleRate = (int)file.Header.sampleRate;
            var frames = frameMaker.ToFrames(file.LeftChannel, sampleRate);
            return extractor.GetFeatures(frames, sampleRate);
        }

        private string[] GetDirs(string dataset)
        {
            var dirs = Directory.GetDirectories(_sampleDirectory + dataset + @"\algorithm");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            return dirs;
        }

        private class Result
        {
            public int FilterBanks { get; set; }
            public int CoeffsLeft { get; set; }
            public Window Window { get; set; }
            public float FrameLength { get; set; }
            public List<double> Averages { get; set; }
        }

    }


}
