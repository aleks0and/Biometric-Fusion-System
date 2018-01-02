using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    /// <summary>
    /// class removing silence from a wav recording
    /// </summary>
    public class SilenceRemoval
    {
        private readonly short _silenceThreshold;
        private int _neighbours;
        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="silenceThreshold">absolute threshold below which samples are cut out</param>
        /// <param name="neighbours">number of neighbours(on one side) included in calcuation of the mean in RemoveSilence</param>
        public SilenceRemoval(short silenceThreshold, int neighbours)
        {
            _silenceThreshold = silenceThreshold;
            _neighbours = neighbours;
        }
        /// <summary>
        /// Removes silence from the recording by cutting out samples where average is below the silence threshold
        /// </summary>
        /// <param name="samples">recording samples including silence</param>
        /// <returns>recording samples without silence</returns>
        public List<short> RemoveSilence(List<short> samples)
        {
            List<short> samplesCopy = new List<short>(samples);
            int average = GetStartingAverage(samples);
            int divisor = _neighbours + 1;

            for(int i = 0; i < samples.Count; i++)
            {
                if(i > _neighbours + 1)
                {
                    average *= divisor;
                    average -= (int)Math.Abs((double)samples[i - (_neighbours + 1)]);
                    divisor--;
                    average /= divisor;
                }
                if(i < samples.Count - _neighbours - 1)
                {
                    average *= divisor;
                    average += (int)Math.Abs((double)samples[i + _neighbours + 1]);
                    divisor++;
                    average /= divisor;
                }
                samplesCopy[i] = (short)average;
            }
            int index = samples.Count;
            for(int i = samples.Count - 1; i >= 0; i--)
            {
                if(samplesCopy[i] < _silenceThreshold)
                {
                    index = i;
                }
                else
                {
                    break;
                }
            }
            if(index != samples.Count)
            {
                samples.RemoveRange(index, samples.Count - index);
            }
            index = -1;
            for(int i = 0; i < samples.Count; i++)
            {
                if(samplesCopy[i] < _silenceThreshold)
                {
                    index = i;
                }
                else
                {
                    break;
                }
            }
            if(index != -1)
            {
                samples.RemoveRange(0, index);
            }
            return samples;
        }
        /// <summary>
        /// Gets starting average for calculating further neighbour average for next samples
        /// </summary>
        /// <param name="samples">samples of wav recording</param>
        /// <returns></returns>
        private short GetStartingAverage(List<short> samples)
        {
            int average = 0;
            for(int i = 0; i < _neighbours + 1; i++)
            {
                average += (int)Math.Abs((double)samples[i]);
            }

            return (short)(average / (_neighbours + 1));
        }
    }
}
