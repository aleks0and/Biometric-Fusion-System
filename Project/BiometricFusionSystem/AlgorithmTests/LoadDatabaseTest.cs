using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmTests;
using FaceRecognition;
using System.Drawing;
using SpeechRecognition;
using Camera;
using System.IO;

namespace AlgorithmTests
{
    [TestClass]
    public class LoadDatabaseTest
    {
        private string speechFilePath = @"..\..\..\..\..\Documentation\speechResult.txt";
        private int _finalMoment;
        private FaceFeatureExtractor _extractorFace;
        private FrameMaker _frameMaker;
        private SpeechFeatureExtractor _extractorSpeech;
        private DynamicTimeWarping _timeWarping;
        private string _sampleDirectory = @"C:\Users\Kornel\Desktop\";

        public LoadDatabaseTest()
        {
            _finalMoment = 3;
            _extractorFace = new FaceFeatureExtractor(_finalMoment);
            _frameMaker = new FrameMaker(frameLength: 0.05f, frameInterval: 0.025f);
            _extractorSpeech = new SpeechFeatureExtractor(window: new HammingWindow(), filterbanksCount: 10);
            _timeWarping = new DynamicTimeWarping(threshold: 0.25);
        }

        [TestMethod]
        public void LoadPersons()
        {
            var dirs = Directory.GetDirectories(_sampleDirectory + @"\faces");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            var classifier = new MinimumDistanceClassifier();
            DbConnection db = new DbConnection();
            var personRepository = new PersonRepository(db);
            DynamicTimeWarping dtwAlgorithm = new DynamicTimeWarping(threshold: 0.25);
            DynamicTimeWarping dtwClose = new DynamicTimeWarping(threshold: 0.25);
            foreach (var dir in dirs)
            {
                var person = new Person();
                List<List<double>> faceVectors = new List<List<double>>();
                for(int i = 0; i < 4; i++)
                {
                    if (File.Exists(_sampleDirectory + @"\faces\" + dir + @"\" + dir + (i + 1) + ".bmp"))
                    { 
                        var faceFeatureVector = GetFeaturesFace(dir + (i + 1) + ".bmp", _sampleDirectory + @"\faces\" + dir + @"\");
                        faceVectors.Add(faceFeatureVector);
                    }
                }
                classifier.AddToDictionary(faceVectors, dir);
                person.FaceFeatureVector = classifier.Classes[dir];
                List<List<double>> speechAVectors = new List<List<double>>();
                for (int i = 0; i < 4; i++)
                {
                    if (File.Exists(_sampleDirectory + @"\algorithm\" + dir + @"\" + dir + (i + 1) + ".wav"))
                    {
                        var faceFeatureVector = GetFeaturesVoice(_sampleDirectory + @"\algorithm\" + dir + @"\" + dir + (i + 1) + ".wav");
                        speechAVectors.Add(faceFeatureVector);
                    }
                }
                dtwAlgorithm.AddToDictionary(speechAVectors, dir);
                List<List<double>> speechCVectors = new List<List<double>>();
                for (int i = 0; i < 4; i++)
                {
                    if (File.Exists(_sampleDirectory + @"\close\" + dir + @"\" + dir + (i + 1) + ".wav"))
                    {
                        var faceFeatureVector = GetFeaturesVoice(_sampleDirectory + @"\close\" + dir + @"\" + dir + (i + 1) + ".wav");
                        speechCVectors.Add(faceFeatureVector);
                    }
                }
                dtwClose.AddToDictionary(speechCVectors, dir);
                person.VoiceFeatureVector = dtwAlgorithm.Classes[dir];
                person.FirstName = dir[0].ToString();
                person.LastName = dir.Substring(1);
                personRepository.AddPerson(person, "algorithm");
                person.VoiceFeatureVector = dtwClose.Classes[dir];
                personRepository.AddPerson(person, "close");
            }
        }

        [TestMethod]
        public void LoadPeopleWithVoiceNormalization()
        {
            var dirs = Directory.GetDirectories(_sampleDirectory + @"\faces");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            var classifier = new MinimumDistanceClassifier();
            DbConnection db = new DbConnection();
            var personRepository = new PersonRepository(db);
            DynamicTimeWarping dtwAlgorithm = new DynamicTimeWarping(threshold: 0.25);
            DynamicTimeWarping dtwClose = new DynamicTimeWarping(threshold: 0.25);
            foreach (var dir in dirs)
            {
                var person = new Person();
                List<List<double>> faceVectors = new List<List<double>>();
                for (int i = 0; i < 4; i++)
                {
                    if (File.Exists(_sampleDirectory + @"\faces\" + dir + @"\" + dir + (i + 1) + ".bmp"))
                    {
                        var faceFeatureVector = GetFeaturesFace(dir + (i + 1) + ".bmp", _sampleDirectory + @"\faces\" + dir + @"\");
                        faceVectors.Add(faceFeatureVector);
                    }
                }
                classifier.AddToDictionary(faceVectors, dir);
                person.FaceFeatureVector = classifier.Classes[dir];
                List<List<double>> speechAVectors = new List<List<double>>();
                for (int i = 0; i < 4; i++)
                {
                    if (File.Exists(_sampleDirectory + @"\algorithm\" + dir + @"\" + dir + (i + 1) + ".wav"))
                    {
                        var faceFeatureVector = GetFeaturesVoice(_sampleDirectory + @"\algorithm\" + dir + @"\" + dir + (i + 1) + ".wav", true);
                        speechAVectors.Add(faceFeatureVector);
                    }
                }
                dtwAlgorithm.AddToDictionary(speechAVectors, dir);
                List<List<double>> speechCVectors = new List<List<double>>();
                for (int i = 0; i < 4; i++)
                {
                    if (File.Exists(_sampleDirectory + @"\close\" + dir + @"\" + dir + (i + 1) + ".wav"))
                    {
                        var faceFeatureVector = GetFeaturesVoice(_sampleDirectory + @"\close\" + dir + @"\" + dir + (i + 1) + ".wav", true);
                        speechCVectors.Add(faceFeatureVector);
                    }
                }
                dtwClose.AddToDictionary(speechCVectors, dir);
                person.VoiceFeatureVector = dtwAlgorithm.Classes[dir];
                person.FirstName = dir[0].ToString();
                person.LastName = dir.Substring(1);
                personRepository.AddPerson(person, "algorithm");
                person.VoiceFeatureVector = dtwClose.Classes[dir];
                personRepository.AddPerson(person, "close");
            }
        }


        public List<double> GetFeaturesFace(string filePath, string directoryPath)
        {
            string path = directoryPath + filePath;
            var bmp = new Bitmap(path);
            var features = _extractorFace.GetFeatureVector(bmp);
            bmp.Dispose();
            return features;
        }

        public List<double> GetFeaturesVoice(string filePath)
        {
            string path = filePath;
            var file = WavReader.Read(path);
            var sampleRate = (int)file.Header.sampleRate;

            var frames = _frameMaker.ToFrames(file.LeftChannel, sampleRate);

            return _extractorSpeech.GetFeatures(frames, sampleRate);
        }
        public List<double> GetFeaturesVoice(string filePath, bool Normalize)
        {

            string path = filePath;
            var file = WavReader.Read(path);
            var sampleRate = (int)file.Header.sampleRate;
            if (Normalize)
            {
                VolumeNormalizer normalizer = new VolumeNormalizer(0, 1000);
                normalizer.Normalize(file.LeftChannel);
            }
            var frames = _frameMaker.ToFrames(file.LeftChannel, sampleRate);
            return _extractorSpeech.GetFeatures(frames, sampleRate);
        }

        [TestMethod]
        public void SpeechTestToFile()
        {
            string testComment = "";
            string dataset = "data";
            string word = "close";
            int filterBanks = 26;
            int coeffs = 13;
            int testFilesPerPerson = 2;
            int trainFilesPerPerson = 4;
            float frameLength = 0.02f, frameInterval = 0.01f;
            Window window = new HammingWindow();
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
                    if (File.Exists(_sampleDirectory + dataset + @"\" + word + @"\" + dir + @"\" + dir + (i + trainFilesPerPerson + 1) + ".wav"))
                    {
                        var path = _sampleDirectory + dataset + @"\" + word + @"\" + dir + @"\" + dir + (i + trainFilesPerPerson + 1) + ".wav";
                        var featureVector = ExtractFeaturesVoice(path, extractor, frameMaker);
                        var result = dtw.Classify(featureVector);
                        bool success = SaveSpeechResult(result, extractor, frameMaker, dir, word,
                            firstRun, i + trainFilesPerPerson + 1, dataset, testComment);
                        if(success)
                        {
                            countSuccess++;
                        }
                        countTotal++;
                        firstRun = false;
                    }
                }
            }
            SaveTotalResult(speechFilePath, countSuccess, countTotal);
        }
        private bool SaveSpeechResult(string result, SpeechFeatureExtractor extractor, FrameMaker frameMaker, string className,
            string word, bool firstRun, int fileId, string dataset, string comment)
        {
            using (var resultFile = new StreamWriter(speechFilePath, true))
            {
                if(firstRun)
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
                resultFile.WriteLine("Input file: {0}, input class: {1}, result class: {2}, {3}", className + fileId, className, result,
                    result == className ? "SUCCESS" : "FAILURE");
            }
            return result == className;
        }

        private void SaveTotalResult(string resultPath, int success, int total)
        {
            using (var resultFile = new StreamWriter(resultPath, true))
            {
                double percent = success / (double)total * 100;
                resultFile.WriteLine("RESULT: {0}/{1} ({2})", success, total, percent.ToString("F2"));
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
    }
}
