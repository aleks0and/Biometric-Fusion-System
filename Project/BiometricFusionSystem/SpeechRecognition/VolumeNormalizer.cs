using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class VolumeNormalizer
    {
        private readonly short _minVolume;
        private readonly short _maxVolume;

        public VolumeNormalizer(short minVolume, short maxVolume)
        {
            _minVolume = minVolume;
            _maxVolume = maxVolume;
        }
        public List<short> Normalize(List<short> samples)
        {
            short min = samples.Min(n => Math.Abs(n));
            short max = samples.Max(n => Math.Abs(n));
            for(int i = 0; i < samples.Count; i++)
            {
                samples[i] = Remap(samples[i], min, max);
            }
            return samples;
        }

        private short Remap(short value, short preMin, short preMax)
        {
            double val = (value - preMin) / (double)(preMax - preMin);
            return (short)(val * (_maxVolume - _minVolume) + _minVolume);
        }
    }
}
