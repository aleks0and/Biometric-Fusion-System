using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class Frame
    {
       public List<short> Samples { get; set; }

       public Frame(List<short> _samples)
       {
            Samples = _samples;
       }
    }
}
