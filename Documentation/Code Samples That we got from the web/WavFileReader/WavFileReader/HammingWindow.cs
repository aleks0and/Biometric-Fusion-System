using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class HammingWindow : Window
    {
        private const float Alpha = 0.53836f;
        private const float Beta = 0.46163f;

        public void ApplyWindow(Frame frame)
        {
            double hamming;
            int sampleLength = frame.Samples.Count;
            for ( int i = 0; i < sampleLength; i++)
            {
                hamming = Alpha - Beta * Math.Cos((2 * Math.PI * (i + 1)) / (sampleLength - 1));
                hamming = hamming * frame.Samples[i];
                frame.Samples[i] = (short)hamming;
            }
        }
    }
}
