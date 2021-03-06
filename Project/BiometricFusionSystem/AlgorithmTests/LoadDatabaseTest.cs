﻿using Common;
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
        private string _sampleDirectory = @"C:\Users\Kornel\Desktop\data_all\data60";

        public LoadDatabaseTest()
        {
            _finalMoment = 3;
            _extractorFace = new FaceFeatureExtractor(_finalMoment);
            _frameMaker = new FrameMaker(frameLength: 0.03f, frameInterval: 0.015f);
            _extractorSpeech = new SpeechFeatureExtractor(window: new HammingWindow(), filterbanksCount: 18, coeffsLeft: 11);
            _timeWarping = new DynamicTimeWarping(threshold: 0.25);
        }

        [TestMethod]
        public void LoadPersons()
        {
            var dirs = Directory.GetDirectories(_sampleDirectory + @"\faces");
            dirs = dirs.Select(d => Path.GetFileName(d)).ToArray();
            var classifier = new MinimumDistanceClassifier();
            DbConnection db = new DbConnection(true);
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
                personRepository.AddPersonTest(person, "algorithm");
                person.VoiceFeatureVector = dtwClose.Classes[dir];
                personRepository.AddPersonTest(person, "close");
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
                personRepository.AddPersonTest(person, "algorithm");
                person.VoiceFeatureVector = dtwClose.Classes[dir];
                personRepository.AddPersonTest(person, "close");
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
        public void GetAllWords()
        {
            var connection = new DbConnection(true);
            var personRepo = new PersonRepository(connection);
            var words = personRepo.SelectAllWords();
            string[] expectedWords = new string[] { "algorithm", "close" };
            foreach(var expected in expectedWords)
            {
                Assert.IsTrue(words.Contains(expected));
            }
        }
        [TestMethod]
        public void AddNewPersonToDb()
        {
            var connection = new DbConnection(true);
            var personRepo = new PersonRepository(connection);
            Person person = new Person()
            {
                FirstName = "TEST",
                LastName = "TEST",
                FaceFeatureVector = new List<double>() { 0 },
                VoiceFeatureVector = new List<double>() { 0 }
            };
            bool result = personRepo.AddPersonTest(person, "algorithm");
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void AddExistingPersonToDb()
        {
            var connection = new DbConnection(true);
            var personRepo = new PersonRepository(connection);
            Person person = new Person()
            {
                FirstName = "TEST1",
                LastName = "TEST1",
                FaceFeatureVector = new List<double>() { 0 },
                VoiceFeatureVector = new List<double>() { 0 }
            };
            personRepo.AddPersonTest(person, "algorithm");
            bool result = personRepo.AddPersonTest(person, "close");
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void AddSpeechToExistingPerson()
        {
            var connection = new DbConnection(true);
            var personRepo = new PersonRepository(connection);
            Person person = new Person()
            {
                FirstName = "TEST2",
                LastName = "TEST2",
                FaceFeatureVector = new List<double>() { 0 },
                VoiceFeatureVector = new List<double>() { 0 }
            };
            personRepo.AddPersonTest(person, "algorithm");
            bool result = personRepo.AddSpeechToExistingPerson(person, "close");
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void AddExistingSpeechToExistingPerson()
        {
            var connection = new DbConnection(true);
            var personRepo = new PersonRepository(connection);
            Person person = new Person()
            {
                FirstName = "TEST3",
                LastName = "TEST3",
                FaceFeatureVector = new List<double>() { 0 },
                VoiceFeatureVector = new List<double>() { 0 }
            };
            personRepo.AddPersonTest(person, "algorithm");
            bool result = personRepo.AddSpeechToExistingPerson(person, "algorithm");
        }
    }
}
