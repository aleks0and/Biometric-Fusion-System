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
        private double _lowerFreq;
        private double _upperFreq;
        int _filterbanksCount = 10;
        int _samplerate;
        int _fourierLength;
        List<double> _filterbanks;
        List<int> _filters;
        List<double> _energies;

        public MelFilterbank(double lowerFreq, double upperFreq, int filterbanksCount, int samplerate, int fourierLength)
        {
            _lowerFreq = MelConverter.ToMel(lowerFreq);
            _upperFreq = MelConverter.ToMel(upperFreq);
            
            _filterbanksCount = filterbanksCount;
            _samplerate = samplerate;
            _fourierLength = fourierLength;

            _filterbanks = new List<double>();
            _filters = new List<int>();
            _energies = new List<double>();
        }

        public void GenerateFilterbanks()
        {
            int space = (int)((_upperFreq - _lowerFreq) / 10);
            var freq = _lowerFreq;
            _filterbanks.Add(_lowerFreq);
            for (int i = 0; i < _filterbanksCount; i++)
            {
                freq += space;
                _filterbanks.Add(freq);
            }
            _filterbanks.Add(_upperFreq);
        }

        public void ConvertFilterbanks()
        {
            for(int i = 0; i < _filterbanks.Count; i++)
            {
                _filterbanks[i] = MelConverter.ToFrequency(_filterbanks[i]);
            }
        }

        public void CalculateFilters()
        {
            for(int i = 0; i < _filterbanks.Count; i++)
            {
                _filters.Add((int)Math.Floor((_fourierLength + 1) * _filterbanks[i] / _samplerate));
            }
        }
        
        public List<double> CreateFilterbank(int m)
        {
            List<double> fb = new List<double>();
            double f;
            for(int k = 0; k <= _filters.Max(); k++)
            {
                if (k < _filters[m - 1])
                    f = 0;
                else if (_filters[m - 1] <= k && _filters[m] >= k)
                    f = (k - _filters[m - 1]) / (_filters[m] - _filters[m - 1]);
                else if (_filters[m] <= k && _filters[m + 1] >= k)
                    f = (_filters[m + 1] - k) / (_filters[m + 1] - _filters[m]);
                else
                    f = 0;
                fb.Add(f);
            }
            return fb;
        }

        public List<List<double>> CreateFilterbanks()
        {
            List<List<double>> fbs = new List<List<double>>();
            for(int i = 1; i < _filterbanks.Count - 1; i++)
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
                energy = Math.Log(energy);
                _energies.Add(energy);
            }
        }

        public List<double> DiscreteCosineTransform()
        {
            List<double> dct = new List<double>();
            
            for(int k = 0; k < _energies.Count; k++)
            {
                double sum = 0;
                for (int n = 0; n < _energies.Count; n++)
                {
                    sum += _energies[n] * Math.Cos(Math.PI / _energies.Count * (n + 1 / 2) * k);
                }
                dct.Add(sum);
            }
            _energies.Clear();
            return dct;
        }
    }
}
