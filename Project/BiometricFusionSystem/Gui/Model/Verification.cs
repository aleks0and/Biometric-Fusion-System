using Common;
using FaceRecognition;
using Gui.Utility;
using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    class Verification
    {
        private Person _personTemplate;
        private PersonRepository _personRepository;
        private MinimumDistanceClassifier _mdc;
        private FrameMaker _frameMaker;
        private SpeechFeatureExtractor _speechFeatureExtractor;
        private DynamicTimeWarping _dynamicTimeWarping;
        private double _faceThreshold;
        private double _voiceThreshold;

        public Verification(DbConnection dbConnection, double faceThreshold, double voiceThreshold)
        {
            _personRepository = new PersonRepository(dbConnection);
            _mdc = new MinimumDistanceClassifier();
            _frameMaker = new FrameMaker(frameLength: 0.03f, frameInterval: 0.015f);
            _speechFeatureExtractor = new SpeechFeatureExtractor(window: new HammingWindow(), filterbanksCount: 18, coeffsLeft: 11);
            _dynamicTimeWarping = new DynamicTimeWarping(threshold: 0);
            _faceThreshold = faceThreshold;
            _voiceThreshold = voiceThreshold;
        }

        public bool VerifySpeech(List<short> samples, int sampleRate)
        {
            bool result = false;

            if (_personTemplate != null)
            {
                var frames = _frameMaker.ToFrames(samples, sampleRate);
                var featureVector = _speechFeatureExtractor.GetFeatures(frames, sampleRate);

                double distance = _dynamicTimeWarping.Compare(featureVector, _personTemplate.VoiceFeatureVector);
                
                if(distance < _voiceThreshold)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool VerifyFace(Bitmap bitmap)
        {
            bool result = false;

            if (_personTemplate != null)
            {
                FaceFeatureExtractor faceFeatureExtractor = new FaceFeatureExtractor(3);
                var fv = faceFeatureExtractor.GetFeatureVector(bitmap);

                double distance = _mdc.FindEuclideanDistance(fv, _personTemplate.FaceFeatureVector);

                if (distance < _faceThreshold)
                {
                    result = true;
                }
            }

            return result;
        }

        public Tuple<bool, bool> Verify(PersonData input, VerificationMethod verificationMethod, string word)
        {
            bool faceResult = false;
            bool speechResult = false;

            _personTemplate = _personRepository.GetPerson(input.FirstName, input.LastName, word);

            if (verificationMethod == VerificationMethod.FaceOnly || verificationMethod == VerificationMethod.FaceAndSpeech)
            {
                var bitmap = BitmapSourceToBitmapConverter.Convert(input.Image);
                faceResult = VerifyFace(bitmap);
            }
            if (verificationMethod == VerificationMethod.SpeechOnly || verificationMethod == VerificationMethod.FaceAndSpeech)
            {
                speechResult = VerifySpeech(input.Samples, (int)input.SampleRate);
            }

            return new Tuple<bool, bool>(faceResult, speechResult);
        }
    }
}
