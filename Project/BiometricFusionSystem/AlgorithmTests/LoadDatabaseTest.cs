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
        private int _finalMoment;
        private FaceFeatureExtractor _extractorFace;
        private FrameMaker _frameMaker;
        private SpeechFeatureExtractor _extractorSpeech;
        private DynamicTimeWarping _timeWarping;
        private string _sampleDirectory = @"C:\Users\Kornel\Desktop\Samples";

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
            var dirs = Directory.GetDirectories(_sampleDirectory + @"/photos");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            var classifier = new MinimumDistanceClassifier();
            DbConnection db = new DbConnection();
            var personRepository = new PersonRepository(db);
            foreach (var dir in dirs)
            {
                List<List<double>> faceVectors = new List<List<double>>();
                for(int i = 0; i < 4; i++)
                {
                    var faceFeatureVector = GetFeaturesFace(dir + (i + 1) + ".bmp", _sampleDirectory + @"\photos\" + dir + @"\");
                    faceVectors.Add(faceFeatureVector);
                }
                classifier.AddToDictionary(faceVectors, dir);
                var person = new Person();
                person.FaceFeatureVector = classifier.Classes[dir];
                person.VoiceFeatureVector = GetFeaturesVoice(_sampleDirectory + @"\algorithm\" + dir + @"\" + dir + "1.wav");
                person.FirstName = dir[0].ToString();
                person.LastName = dir.Substring(1);
                personRepository.AddPerson(person, "algorithm");
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
    }
}
