using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class GaussWindow : Window
    {
        /// <summary>
        ///  Sigma is the standard deviation of this function
        /// </summary>
        private const float Sigma = 0.4f;
        //private const float B = 1.45f; Possibly used for fourier transform
        /// <summary>
        /// Function which applies the Gaussian window which amplifies the middle of the frame and weakens the beginning and the end of the frame.
        /// </summary>
        /// <param name="frame"> Frame to which the window will be applied</param>
        public void ApplyWindow(Frame frame)
        {
            double gauss;
            int sampleLength = frame.Samples.Count;
            for (int i = 0; i < sampleLength; i++)
            {
                gauss = Math.Exp((-0.5) * Math.Pow((((i + 1) - (sampleLength - 1) / 2) / (Sigma * (sampleLength - 1) / 2)), 2));
                gauss = gauss * frame.Samples[i];
                frame.Samples[i] = (short)gauss;
            }
        }
    }
}
