using Common;
using FaceRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    class Identification
    {
        List<Person> persons;
        PersonRepository personRepository;
        MinimumDistanceClassifier mdc;

        public Identification(DbConnection dbConnection)
        {
            persons = new List<Person>();
            personRepository = new PersonRepository(dbConnection);
            mdc = new MinimumDistanceClassifier();
        }

        public void LoadPersonsList()
        {
            if(persons.Count == 0)
            {
                persons = personRepository.SelectPersons();
            }
            for(int i = 0; i < persons.Count; i++)
            {
                mdc.Classes.Add(persons[i].FirstName + persons[i].LastName, persons[i].FaceFeatureVector);
            }
        }

        public void IdentifySpeech()
        {

        }

        public string IdentifyFace(Bitmap faceImage)
        {
            FaceFeatureExtractor faceFeatureExtractor = new FaceFeatureExtractor(3);
            var fv = faceFeatureExtractor.GetFeatureVector(faceImage);
            
            string className = mdc.Classify(fv);
            return className;
        }

        public void Identify()
        {

        }
    }
}
