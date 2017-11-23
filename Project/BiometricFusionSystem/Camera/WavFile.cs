using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    public class WavFile
    {
        public List<short> LeftChannel { get; set; }
        public List<short> RightChannel { get; set; }
        public WavHeader Header { get; set; }
    }
}
