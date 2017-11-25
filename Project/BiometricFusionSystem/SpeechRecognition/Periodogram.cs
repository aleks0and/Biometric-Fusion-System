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
        /// <summary>
        /// function calculating the periodogram estimates for the frame. which 
        /// </summary>
        /// <param name="frame"> frame for which the estimates will be calculated</param>
        /// <returns> estimates stored as list of double </returns>
        public static List<double> GetEstimate(Frame frame)
        {
            var fourierCoeffs = FourierTransform.Apply(frame.Samples);

            var estimates = new List<double>();
            //take just first half of fourier coefficients
            for(int i = 0; i < fourierCoeffs.Length / 2; i++)
            {
                var powerSpectrum = Math.Pow(fourierCoeffs[i].Magnitude, 2) / frame.Samples.Count;
                estimates.Add(powerSpectrum);
            }
            return estimates;
        }
    }
}
