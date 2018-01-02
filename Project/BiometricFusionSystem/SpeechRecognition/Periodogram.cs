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
        /// function applying FFT to a frame, what results with the calculation of n (length of the frame) coefficients which are later 
        /// used for calculation of spectral density estimates. We only use first half of coefficients as the second half is its repetition.
        /// </summary>
        /// <param name="frame"> frame for which the estimates will be calculated </param>
        /// <returns> spectral density estimates stored as list of double </returns>
        public static List<double> GetEstimate(Frame frame)
        {
            var fourierCoeffs = FourierTransform.Apply(frame.Samples);
            var estimates = new List<double>();
            for(int i = 0; i < fourierCoeffs.Length / 2; i++)
            {
                var powerSpectrum = Math.Pow(fourierCoeffs[i].Magnitude, 2) / frame.Samples.Count;
                estimates.Add(powerSpectrum);
            }
            return estimates;
        }
    }
}
