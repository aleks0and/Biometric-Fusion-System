using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    /// <summary>
    /// Class holding the header values for the wav. files 
    /// LeftChannel corresponds to the left channel of the audio stored in the recording
    /// RightChannel corresponds to the right channel of the audio stored in the recording
    /// Header holds the WavHeader class which represents all the information stored in the header of wav. file
    /// </summary>
    public class WavFile
    {
        public List<short> LeftChannel { get; set; }
        public List<short> RightChannel { get; set; }
        public WavHeader Header { get; set; }
    }
}
