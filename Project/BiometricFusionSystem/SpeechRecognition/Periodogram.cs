using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class Periodogram
    {
        public static List<double> GetEstimate(Frame frame)
        {
            var fourierCoeffs = FourierTransform.Apply(frame.Samples);

            var estimates = new List<double>();
            for(int i = 0; i < fourierCoeffs.Length; i++)
            {
                var powerSpectrum = Math.Pow(fourierCoeffs[i].Magnitude, 2) / frame.Samples.Count;
                estimates.Add(powerSpectrum);
            }
            return estimates;
        }
    }
}
