using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class MelConverter
    {
        public static double ToMel(double freq)
        {
            double mel = 1125 * Math.Log(1 + freq / 700);
            return mel;
        }

        public static double ToFrequency(double mel)
        {
            double freq = 700 * (Math.Exp(mel / 1125) - 1);
            return freq;
        }
    }
}
