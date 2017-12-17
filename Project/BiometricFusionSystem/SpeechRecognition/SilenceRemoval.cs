using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class SilenceRemoval
    {
        private readonly short _silenceThreshold;
        private int _neighbours;

        public SilenceRemoval(short silenceThreshold, int neighbours)
        {
            _silenceThreshold = silenceThreshold;
            _neighbours = neighbours;
        }

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
                    average -= Math.Abs(samples[i - (_neighbours + 1)]);
                    divisor--;
                    average /= divisor;
                }
                if(i < samples.Count - _neighbours - 1)
                {
                    average *= divisor;
                    average += Math.Abs(samples[i + _neighbours + 1]);
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
        private short GetStartingAverage(List<short> samples)
        {
            int average = 0;
            for(int i = 0; i < _neighbours + 1; i++)
            {
                average += Math.Abs(samples[i]);
            }

            return (short)(average / (_neighbours + 1));
        }
    }
}
