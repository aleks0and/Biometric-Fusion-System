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
            Person person = new Person();
            
            person.FirstName = "A";
            person.LastName = "B";
            List<List<double>> fvs = new List<List<double>>();
            for(int i = 0; i < 4; i++)
            {
                List<double> fv = GetFeaturesFace("AB" + (i + 1) + ".bmp", @"C:\Users\Martyna\Desktop\Samples\AB\");
                fvs.Add(fv);
            }
            MinimumDistanceClassifier minimumDistanceClassifier = new MinimumDistanceClassifier();
            minimumDistanceClassifier.AddToDictionary(fvs, "AB");
            person.FaceFeatureVector = minimumDistanceClassifier._classes["AB"];

            person.VoiceFeatureVector = GetFeaturesVoice(@"C:\Users\Martyna\Desktop\close\AB\AB1.wav");

            DbConnection db = new DbConnection();

            PersonRepository personRepository = new PersonRepository(db);
            personRepository.AddPerson(person, "close");

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
