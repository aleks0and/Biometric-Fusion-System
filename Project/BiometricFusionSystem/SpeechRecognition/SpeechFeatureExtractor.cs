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

        public List<double> GetFeatures(List<Frame> frames, int sampleRate)
        {
            ApplyWindow(frames);
            var estimates = GetPeriodogramEstimates(frames);
            var melFilterbank = new MelFilterbank(LowerFrequency, sampleRate / 2,
                _filterbanksCount, sampleRate, estimates[0].Count);
            var filterbanks = PrepareFilterbanks(melFilterbank);

            return GetMfcc(melFilterbank, filterbanks, estimates);
        }

        private void ApplyWindow(List<Frame> frames)
        {
            for(int i = 0; i < frames.Count; i++)
            {
                _windowFunction.ApplyWindow(frames[i]);
            }
        }
        private List<List<double>> GetPeriodogramEstimates(List<Frame> frames)
        {
            var estimates = new List<List<double>>();
            for(int i = 0; i < frames.Count; i++)
            {
                estimates.Add(Periodogram.GetEstimate(frames[i]));
            }

            return estimates;
        }

        private List<List<double>> PrepareFilterbanks(MelFilterbank filterbank)
        {
            filterbank.GenerateFilterbankIntervals();
            filterbank.ConvertFilterbanks();
            filterbank.CalculateFilters();

            return filterbank.CreateFilterbanks();
        }

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
