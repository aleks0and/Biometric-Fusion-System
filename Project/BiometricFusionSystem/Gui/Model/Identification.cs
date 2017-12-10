using Common;
using FaceRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Identification(DbConnection dbConnection)
        {
            _persons = new List<Person>();
            _personRepository = new PersonRepository(dbConnection);
            _mdc = new MinimumDistanceClassifier();
            _frameMaker = new FrameMaker(frameLength: 0.05f, frameInterval: 0.025f);
            _speechFeatureExtractor = new SpeechFeatureExtractor(window: new HammingWindow(), filterbanksCount: 10);
            _dynamicTimeWarping = new DynamicTimeWarping(threshold: 0.25);
        }

        public void LoadPersonsList()
        {
            if(_persons.Count == 0)
            {
                _persons = _personRepository.SelectPersons();
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

        public Tuple<string, string> Identify(PersonData person)
        {
            LoadPersonsList();

            var bitmap = BitmapSourceToBitmapConverter.Convert(person.Image);
            var faceResult = IdentifyFace(bitmap);
            var speechResult = IdentifySpeech(person.Samples, (int)person.SampleRate);

            return new Tuple<string, string>(faceResult, speechResult);
        }
    }
}
