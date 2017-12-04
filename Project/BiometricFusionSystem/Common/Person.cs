using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Class holding the information neccessary for person verification/identification
    /// Id - Id number in the database
    /// First name
    /// Last name
    /// FaceImage - image of the face of the person stored as an byte array
    /// FaceFeatureVector - features extracted from the picture stored as list of double
    /// VoiceRecording - recording of speech of the person stored as an byte array
    /// VoiceFeatureVector - features extracted from the recording sotred as list of double
    /// </summary>
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] FaceImage { get; set; }
        public List<double> FaceFeatureVector { get; set; }
        public byte[] VoiceRecording { get; set; }
        public List<double> VoiceFeatureVector { get; set; }
        /// <summary>
        /// Function converting the list of face feature vectors to string separated by a loaded character 
        /// </summary>
        /// <param name="separator"> character which separates the values </param>
        /// <returns> string contating feature vectors </returns>
        public string FaceFeatureVectorToString(char separator)
        {
            return FeatureVectorToString(FaceFeatureVector, separator);
        }
        public string VoiceFeatureVectorToString(char separator)
        {
            return FeatureVectorToString(VoiceFeatureVector, separator);
        }
        private string FeatureVectorToString(List<double> vector, char separator)
        {
            string ffv = "";

            for (int i = 0; i < vector.Count; i++)
            {
                ffv += vector[i].ToString("F2") + separator;
            }

            return ffv;
        }
        /// <summary>
        /// Function converting the string of face feature vectors to list of doubles
        /// </summary>
        /// <param name="featureVectorString"> string with face feature vectors </param>
        /// <param name="separator"> character which separates the values </param>
        /// <returns> list of double contating feature vectors </returns>
        public static List<double> FeatureVectorToList(string featureVectorString, char separator)
        {
            List<double> ffv = new List<double>();

            string[] q = featureVectorString.Split(separator);

            for(int i = 0; i < q.Length; i++)
            {
                double w = double.Parse(q[i]);
                ffv.Add(w);
            }

            return ffv;
        }
    }


}
