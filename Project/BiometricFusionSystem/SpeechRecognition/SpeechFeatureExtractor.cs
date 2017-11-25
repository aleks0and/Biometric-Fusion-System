using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class SpeechFeatureExtractor
    {
        private const int LowerFrequency = 300;
        private Window _windowFunction;
        private int _filterbanksCount;

        public SpeechFeatureExtractor(Window window, int filterbanksCount)
        {
            _filterbanksCount = filterbanksCount;
            _windowFunction = window;
        }
        /// <summary>
        /// function extracting the features from the speech recording split into frames
        /// </summary>
        /// <param name="frames"> speech recording as list of frame</param>
        /// <param name="sampleRate"> samplereate of the recording</param>
        /// <returns> calculated feature vector stored as list of double</returns>
        public List<double> GetFeatures(List<Frame> frames, int sampleRate)
        {
            ApplyWindow(frames);
            var estimates = GetPeriodogramEstimates(frames);
            var melFilterbank = new MelFilterbank(LowerFrequency, sampleRate / 2,
                _filterbanksCount, sampleRate, estimates[0].Count);
            var filterbanks = PrepareFilterbanks(melFilterbank);
            return GetMfcc(melFilterbank, filterbanks, estimates);
        }
        /// <summary>
        /// function applying the window function to the list of frames
        /// </summary>
        /// <param name="frames">speech recording stored as list of frames</param>
        private void ApplyWindow(List<Frame> frames)
        {
            for(int i = 0; i < frames.Count; i++)
            {
                _windowFunction.ApplyWindow(frames[i]);
            }
        }
        /// <summary>
        /// Function calculating the periodogram estimates
        /// </summary>
        /// <param name="frames">speech recording stored as list of frames</param>
        /// <returns>estimates calculated for each frame</returns>
        private List<List<double>> GetPeriodogramEstimates(List<Frame> frames)
        {
            var estimates = new List<List<double>>();
            for(int i = 0; i < frames.Count; i++)
            {
                estimates.Add(Periodogram.GetEstimate(frames[i]));
            }

            return estimates;
        }
        /// <summary>
        /// function applying the neccesary operations for the following use filterbanks:
        /// n + 2 linearly spaced filter intervals are generated
        /// function converts the filter intervals back to Herz
        /// filterbank of n vectors of triangular filters is computed
        /// </summary>
        /// <param name="filterbank"> Mel filterbank </param>
        /// <returns> filterbanks stored as list of double</returns>
        private List<List<double>> PrepareFilterbanks(MelFilterbank filterbank)
        {
            filterbank.GenerateFilterbankIntervals();
            filterbank.ConvertFilterbanks();
            filterbank.CalculateFilters();
            return filterbank.CreateFilterbanks();
        }
        /// <summary>
        /// function which returns the feature vector which values are Mel Frequency Cepstral Coefficients
        /// </summary>
        /// <param name="melFilterbank"> filter bank</param>
        /// <param name="filterbanks"> filterbanks after the preprocessing</param>
        /// <param name="estimates"> periodogram estimates</param>
        /// <returns> feature vector used for speech verification </returns>
        private List<double> GetMfcc(MelFilterbank melFilterbank, List<List<double>> filterbanks,
            List<List<double>> estimates)
        {
            var dct = new List<List<double>>();
            for(int i = 0; i < estimates.Count; i++)
            {
                melFilterbank.CalculateFilterbanksEnergies(estimates[i], filterbanks);
                dct.Add(melFilterbank.DiscreteCosineTransform());
            }

            return dct.SelectMany(n => n).ToList();
        }
    }
}
