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

        public string FaceFeatureVectorToString()
        {
            string ffv = "";

            for(int i = 0; i < FaceFeatureVector.Count; i++)
            {
                ffv += FaceFeatureVector[i] + ",";
            }

            return ffv;
        }

        public List<double> FaceFeatureVectorToList(string strffv)
        {
            List<double> ffv = new List<double>();

            string[] q = strffv.Split(',');

            for(int i = 0; i < q.Length; i++)
            {
                double w = double.Parse(q[i]);
                ffv.Add(w);
            }

            return ffv;
        }
    }


}
