using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MathNet.Numerics;

namespace SpeechRecognition
{
    public class MelFilterbank
    {
        private const double LogErr = 1E-10;
        private double _lowerFreq;
        private double _upperFreq;
        int _filterbanksCount;
        int _samplerate;
        int _fourierLength;
        public List<double> Filterbanks { get; private set; }
        public List<int> Filters { get; private set; }
        public List<double> Energies { get; private set; }


        public MelFilterbank(double lowerFreq, double upperFreq, int filterbanksCount, int samplerate, int fourierLength)
        {
            _lowerFreq = MelConverter.ToMel(lowerFreq);
            _upperFreq = MelConverter.ToMel(upperFreq);
            
            _filterbanksCount = filterbanksCount;
            _samplerate = samplerate;
            _fourierLength = fourierLength;

            Filterbanks = new List<double>();
            Filters = new List<int>();
            Energies = new List<double>();
        }

        public void GenerateFilterbankIntervals()
        {
            double space = (_upperFreq - _lowerFreq) / (_filterbanksCount + 1);
            var freq = _lowerFreq;
            for (int i = 0; i < _filterbanksCount+2; i++)
            {
                Filterbanks.Add(freq);
                freq += space;
            }
        }

        public void ConvertFilterbanks()
        {
            for(int i = 0; i < Filterbanks.Count; i++)
            {
                Filterbanks[i] = MelConverter.ToFrequency(Filterbanks[i]);
            }
        }

        public void CalculateFilters()
        {
            for(int i = 0; i < Filterbanks.Count; i++)
            {
                Filters.Add((int)Math.Floor((2 * _fourierLength + 1) * Filterbanks[i] / _samplerate));
            }
        }
        
        public List<double> CreateFilterbank(int m)
        {
            List<double> fb = new List<double>();
            double f;
            for(int k = 0; k <= Filters.Max(); k++)
            {
                if (k < Filters[m - 1])
                    f = 0;
                else if (Filters[m - 1] <= k && Filters[m] >= k)
                {
                    if ((Filters[m] - Filters[m - 1]) != 0)
                        f = (k - Filters[m - 1]) / (Filters[m] - Filters[m - 1]);
                    else
                        f = 0;
                }
                else if (Filters[m] <= k && Filters[m + 1] >= k)
                {
                    if ((Filters[m + 1] - Filters[m]) != 0)
                        f = (Filters[m + 1] - k) / (Filters[m + 1] - Filters[m]);
                    else
                        f = 0;
                }
                else
                    f = 0;
                fb.Add(f);
            }
            return fb;
        }

        public List<List<double>> CreateFilterbanks()
        {
            List<List<double>> fbs = new List<List<double>>();
            for(int i = 1; i < Filterbanks.Count - 1; i++)
            {
                List<double> f = CreateFilterbank(i);
                fbs.Add(f);
            }
            return fbs;
        }

        public void CalculateFilterbanksEnergies(List<double> est, List<List<double>> fbs)
        {
            for(int i = 0; i < fbs.Count; i++)
            {
                double energy = 0;
                for (int j = 0; j < est.Count; j++)
                {
                    energy += fbs[i][j] * est[j];
                }
                if(energy < LogErr)
                {
                    energy += LogErr;
                }
                energy = Math.Log(energy);
                Energies.Add(energy);
            }
        }

        public List<double> DiscreteCosineTransform()
        {
            List<double> dct = new List<double>();
            
            for(int k = 0; k < Energies.Count; k++)
            {
                double sum = 0;
                for (int n = 0; n < Energies.Count; n++)
                {
                    sum += Energies[n] * Math.Cos(Math.PI / Energies.Count * (n + 1 / 2) * k);
                }
                dct.Add(sum);
            }
            Energies.Clear();
            return dct;
        }
    }
}
