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
    }
}
