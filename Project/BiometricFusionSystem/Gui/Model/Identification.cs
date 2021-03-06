﻿using Common;
using FaceRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using SpeechRecognition;
using Gui.Utility;

namespace Gui.Model
{
    public class Identification
    {
        private List<Person> _persons;
        private PersonRepository _personRepository;
        private MinimumDistanceClassifier _mdc;
        private FrameMaker _frameMaker;
        private SpeechFeatureExtractor _speechFeatureExtractor;
        private DynamicTimeWarping _dynamicTimeWarping;
        private string _lastWord = "";

        public Identification(DbConnection dbConnection)
        {
            _persons = new List<Person>();
            _personRepository = new PersonRepository(dbConnection);
            _mdc = new MinimumDistanceClassifier();
            _frameMaker = new FrameMaker(frameLength: 0.03f, frameInterval: 0.015f);
            _speechFeatureExtractor = new SpeechFeatureExtractor(window: new HammingWindow(), filterbanksCount: 18, coeffsLeft: 11);
            _dynamicTimeWarping = new DynamicTimeWarping(threshold: 0);
        }

        public void LoadPersonsList(string word)
        {
            if(_persons.Count == 0 || _lastWord != word)
            {
                _mdc.Classes.Clear();
                _dynamicTimeWarping.Classes.Clear();
                _persons = _personRepository.SelectPersons(word);
                for (int i = 0; i < _persons.Count; i++)
                {
                    _mdc.Classes.Add(_persons[i].FirstName + _persons[i].LastName, _persons[i].FaceFeatureVector);
                    _dynamicTimeWarping.Classes.Add(_persons[i].FirstName + _persons[i].LastName, _persons[i].VoiceFeatureVector);
                }
            }
        }

        public string IdentifySpeech(List<short> samples, int sampleRate)
        {
            var frames = _frameMaker.ToFrames(samples, sampleRate);
            var featureVector = _speechFeatureExtractor.GetFeatures(frames, sampleRate);

            return _dynamicTimeWarping.Classify(featureVector);
        }

        public string IdentifyFace(Bitmap faceImage)
        {
            FaceFeatureExtractor faceFeatureExtractor = new FaceFeatureExtractor(3);
            var fv = faceFeatureExtractor.GetFeatureVector(faceImage);
            
            return _mdc.Classify(fv);
        }

        public Tuple<string, string> Identify(PersonData person, IdentificationMethod identificationMethod, string word)
        {
            LoadPersonsList(word);
            string faceResult = "no result"; 
            string speechResult = "no result";
            
            if (identificationMethod == IdentificationMethod.FaceOnly || identificationMethod == IdentificationMethod.FaceAndSpeech)
            {
                var bitmap = BitmapSourceToBitmapConverter.Convert(person.Image);
                faceResult = IdentifyFace(bitmap);
            }
            if(identificationMethod == IdentificationMethod.SpeechOnly || identificationMethod == IdentificationMethod.FaceAndSpeech)
            {
                speechResult = IdentifySpeech(person.Samples, (int)person.SampleRate);
            }

            return new Tuple<string, string>(faceResult, speechResult);
        }
    }
}
