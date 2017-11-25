using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpeechRecognition
{
    /// <summary>
    /// Class which stores the frames of a speech recording
    /// </summary>
    public class Frame
    {
       public List<short> Samples { get; set; }
        
       public Frame(List<short> _samples)
       {
            Samples = _samples;
       }
    }
}
