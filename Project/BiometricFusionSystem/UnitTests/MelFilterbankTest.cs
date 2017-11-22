using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechRecognition;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class MelFilterbankTest
    {
        const double LogErr = 1E-10;
        MelFilterbank _filterBank;
        private double _lowerFreq;
        private double _upperFreq;
        int _filterbanksCount;
        int _samplerate;
        int _fourierLength;
        [TestInitialize]
        public void init ()
        {
            _lowerFreq = 300;
            _upperFreq = 4000; 
            _filterbanksCount = 10;
            _samplerate = 8000;
            _fourierLength = 6; //half of the number of frames
            _filterBank = new MelFilterbank(_lowerFreq, _upperFreq, _filterbanksCount, _samplerate, _fourierLength);
        }
        [TestMethod]
        public void FilterbanksAreGenerated()
        {
            double lowerFreqInMel = MelConverter.ToMel(_lowerFreq);
            double upperFreqInMel = MelConverter.ToMel(_upperFreq);
            double step = (upperFreqInMel - lowerFreqInMel) / (_filterbanksCount + 1);
            List<double> expectedResult = new List<double>();
            for (int i = 0; i < _filterbanksCount+2; i++)
            {    
                expectedResult.Add(lowerFreqInMel + step * i);
            }

            _filterBank.GenerateFilterbankIntervals();
            List<double> actualResult = _filterBank.Filterbanks;

            for(int i = 0; i < _filterbanksCount+2; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i], 0.1);
            }
        }
        [TestMethod]
        public void CalculationOfFilters()
        {
            _filterBank.GenerateFilterbankIntervals();
            _filterBank.ConvertFilterbanks();
            _filterBank.CalculateFilters();
            List<int> actualResult = _filterBank.Filters;
            List<int> expectedResult = new List<int>();
            //results calculated according to the math formula
            expectedResult.Add(0);
            expectedResult.Add(0);
            expectedResult.Add(1);
            expectedResult.Add(1);
            expectedResult.Add(1);
            expectedResult.Add(2);
            expectedResult.Add(2);
            expectedResult.Add(3);
            expectedResult.Add(3);
            expectedResult.Add(4);
            expectedResult.Add(5);
            expectedResult.Add(6);
            for (int i = 0; i < _filterbanksCount+2; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i]);
            }
        }
        [TestMethod]
        public void FilterBanksAreCreated1()
        {
            _filterBank.GenerateFilterbankIntervals();
            _filterBank.ConvertFilterbanks();
            _filterBank.CalculateFilters();
            List<double> actualResult = new List<double>();
            actualResult = _filterBank.CreateFilterbank(1);
            List<double> expectedResult = new List<double>();

            expectedResult.Add(0);
            expectedResult.Add(0);
            expectedResult.Add(0);
            expectedResult.Add(0);
            expectedResult.Add(0);
            expectedResult.Add(0);
            expectedResult.Add(0);
            for (int i=0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i]);
            }
        }
        [TestMethod]
        public void FilterBankEnergiesAreCalculated ()
        {
            _filterBank.GenerateFilterbankIntervals();
            _filterBank.ConvertFilterbanks();
            _filterBank.CalculateFilters();
            List<List<double>> filterBanks = _filterBank.CreateFilterbanks();
            List<double> periodogramMock = new List<double>();
            List<double> expectedResult = new List<double>();
            List<double> actualResult = _filterBank.Energies;
            for (int i=0; i< 7; i++)
            {
                periodogramMock.Add(i);
            }
            _filterBank.CalculateFilterbanksEnergies(periodogramMock, filterBanks);
            //1.1, 4.2, 6.3, 8.4, 9.5
            double lnLogErr = Math.Log(LogErr);
            expectedResult.Add(lnLogErr);
            expectedResult.Add(0);
            expectedResult.Add(lnLogErr);
            expectedResult.Add(lnLogErr);
            expectedResult.Add(Math.Log(2));
            expectedResult.Add(lnLogErr);
            expectedResult.Add(Math.Log(3));
            expectedResult.Add(lnLogErr);
            expectedResult.Add(Math.Log(4));
            expectedResult.Add(Math.Log(5));
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i]);
            }
        }
        [TestMethod]
        public void DCTResults()
        {
            _filterBank.GenerateFilterbankIntervals();
            _filterBank.ConvertFilterbanks();
            _filterBank.CalculateFilters();
            List<List<double>> filterBanks = _filterBank.CreateFilterbanks();
            List<double> periodogramMock = new List<double>();
            List<double> expectedResult = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                periodogramMock.Add(i);
            }
            _filterBank.CalculateFilterbanksEnergies(periodogramMock, filterBanks);
            List<double> actualResult = _filterBank.DiscreteCosineTransform();
            
            expectedResult.Add(-110.34);
            expectedResult.Add(-32.64);
            expectedResult.Add(14.43);
            expectedResult.Add(-9.16);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i],0.2);
            }

        }
        [TestMethod]
        public void EnergiesAfterDCTAreCleared()
        {
            _filterBank.GenerateFilterbankIntervals();
            _filterBank.ConvertFilterbanks();
            _filterBank.CalculateFilters();
            List<List<double>> filterBanks = _filterBank.CreateFilterbanks();
            List<double> periodogramMock = new List<double>();
            List<double> expectedResult = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                periodogramMock.Add(i);
            }
            _filterBank.CalculateFilterbanksEnergies(periodogramMock, filterBanks);
            List<double> actualResult = _filterBank.DiscreteCosineTransform();
            Assert.AreEqual(0, _filterBank.Energies.Count);
        }
    }
}
