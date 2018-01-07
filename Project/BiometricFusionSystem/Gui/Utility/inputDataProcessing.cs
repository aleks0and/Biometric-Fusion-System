using Camera;
using Common;
using FaceRecognition;
using Gui.Model;
using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Utility
{
    public class InputDataProcessing
    {
        // Speech feature extraction parameters
        private float _frameLength = 0.03f;
        private float _frameInterval = 0.015f;
        private int _filterBanksCount = 18;
        private int _coefficientCount = 11;
        // Face feature extraction parameters
        private int _finalMoment = 3;
        private int _orientations = 7;
        private double _lambda = 4;
        private double _stdX = 6;
        private double _stdY = 7.5;
        
        private MinimumDistanceClassifier _mdc;
        private FrameMaker _frameMaker;
        private SpeechFeatureExtractor _speechFeatureExtractor;
        private FaceFeatureExtractor _faceFeatureExtractor;
        private DynamicTimeWarping _dynamicTimeWarping;
        private PersonToAdd _person;
        
        public InputDataProcessing(PersonToAdd person)
        {
            _mdc = new MinimumDistanceClassifier();
            _frameMaker = new FrameMaker(_frameLength, _frameInterval);
            _speechFeatureExtractor = new SpeechFeatureExtractor(new HammingWindow(), _filterBanksCount, _coefficientCount);
            _faceFeatureExtractor = new FaceFeatureExtractor(_finalMoment, _lambda, _stdX, _stdY, _orientations);
            _person = person;
        }

        public List<double> extractFaceFeatureVector()
        {
            List<string> images = _person.ImagePathList.ToList();
            List<List<double>> featureVectors = new List<List<double>>();
            foreach (var imagepath in images)
            {
                var file = new Bitmap(imagepath);
                var vector = _faceFeatureExtractor.GetFeatureVector(file);
                file.Dispose();
                featureVectors.Add(vector);
            }
            _mdc.AddToDictionary(featureVectors, _person.FirstName + _person.LastName); // should here be a space between?
            return _mdc.Classes[_person.FirstName + _person.LastName];
        }

        public List<double> extractSpeechFeatureVector()
        {
            List<string> recordings = _person.SpeechPathList.ToList();
            List<List<double>> featureVectors = new List<List<double>>();
            foreach (var record in recordings)
            {
                var file = WavReader.Read(record);
                var sampleRate = (int)file.Header.sampleRate;
                var frames = _frameMaker.ToFrames(file.LeftChannel, sampleRate);
                var vector = _speechFeatureExtractor.GetFeatures(frames, sampleRate);
                featureVectors.Add(vector);
            }
            _dynamicTimeWarping.AddToDictionary(featureVectors, _person.FirstName + _person.LastName);
            return _dynamicTimeWarping.Classes[_person.FirstName + _person.LastName];
        }
    }
    
}
