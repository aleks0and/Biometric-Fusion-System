using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] FaceImage { get; set; }
        public List<double> FaceFeatureVector { get; set; }
        public byte[] VoiceRecording { get; set; }
        public List<double> VoiceFeatureVector { get; set; }

        public static string FeatureVectorToString(List<double> vector)
        {
            string ffv = "";

            for(int i = 0; i < vector.Count; i++)
            {
                ffv += vector[i] + ",";
            }

            return ffv;
        }

        public static List<double> FeatureVectorToList(string vector)
        {
            List<double> ffv = new List<double>();

            string[] q = vector.Split(',');

            for(int i = 0; i < q.Length; i++)
            {
                double w = double.Parse(q[i]);
                ffv.Add(w);
            }

            return ffv;
        }
    }


}
