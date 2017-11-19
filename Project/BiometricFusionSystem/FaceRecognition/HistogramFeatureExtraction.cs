using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public class HistogramFeatureExtraction
    {
        private const int Max = 256;
        int[] _histogramR;
        int[] _histogramG;
        int[] _histogramB;
        int _height;
        int _width;
        Bitmap _bmp;

        public HistogramFeatureExtraction(int width, int height, Bitmap bmp)
        {
            _histogramR = new int[Max];
            _histogramG = new int[Max];
            _histogramB = new int[Max];
            _height = height;
            _width = width;
            _bmp = bmp;
        }

        // second Moment is the Variance, third is the Skewness
        public List<List<double>> CalculateMoments (int finalMoment)
        {
            List<List<double>> moments = new List<List<double>>();
            moments.Add(CalculateMean());
            for (int i = 2; i < finalMoment; i++)
            {
                moments.Add(CalculateIthMoment(i, moments[0]));
            }
            return moments;
            
        }
        private List<double> CalculateMean ()
        {
            double sumR = 0;
            double sumG = 0;
            double sumB = 0;
            List<double> Means = new List<double>();
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    sumR += _bmp.GetPixel(i, j).R;
                    sumG += _bmp.GetPixel(i, j).G;
                    sumB += _bmp.GetPixel(i, j).B;
                }
            }
            int pixelCount = _height * _width;
            sumR /= pixelCount;
            sumG /= pixelCount;
            sumB /= pixelCount;
            Means.Add(sumR);
            Means.Add(sumG);
            Means.Add(sumB);
            return Means;
        }
        private List<double> CalculateIthMoment (int moment, List<double> mean)
        {
            List<double> ithMoment = new List<double>();
            double sumR = 0;
            double sumG = 0;
            double sumB = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    sumR += Math.Pow(_bmp.GetPixel(i, j).R - mean[0], moment);
                    sumG += Math.Pow(_bmp.GetPixel(i, j).G - mean[1], moment);
                    sumB += Math.Pow(_bmp.GetPixel(i, j).B - mean[2], moment);
                }
            }
            int pixelCount = _height * _width;
            sumR /= pixelCount;
            sumG /= pixelCount;
            sumB /= pixelCount;
            sumR = Math.Pow(sumR, 1 / moment);
            sumG = Math.Pow(sumG, 1 / moment);
            sumB = Math.Pow(sumB, 1 / moment);
            ithMoment.Add(sumR);
            ithMoment.Add(sumG);
            ithMoment.Add(sumB);
            return ithMoment;
        }

    }
}
