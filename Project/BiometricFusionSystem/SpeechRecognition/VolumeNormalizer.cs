using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    /// <summary>
    /// Class normalizing volume(amplitude) of each recording to identical max and min level
    /// </summary>
    public class VolumeNormalizer
    {
        private readonly short _minVolume;
        private readonly short _maxVolume;
        /// <summary>
        /// Constructor specifing new amplitude range for processed recordings 
        /// </summary>
        /// <param name="minVolume">new minimum volume of recording</param>
        /// <param name="maxVolume">new maximum volume of recording</param>
        public VolumeNormalizer(short minVolume, short maxVolume)
        {
            _minVolume = minVolume;
            _maxVolume = maxVolume;
        }
        /// <summary>
        /// Function normalizing samples of recording to a new range of amplitude
        /// </summary>
        /// <param name="samples">Samples of .wav recording</param>
        /// <returns>Normalized samples</returns>
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
        /// <summary>
        /// Function remaps value from old range to a new range
        /// </summary>
        /// <param name="value">value to be remapped</param>
        /// <param name="preMin">minimum of old range</param>
        /// <param name="preMax">maximum of old range</param>
        /// <returns></returns>
        private short Remap(short value, short preMin, short preMax)
        {
            double val = (value - preMin) / (double)(preMax - preMin);
            return (short)(val * (_maxVolume - _minVolume) + _minVolume);
        }
    }
}
