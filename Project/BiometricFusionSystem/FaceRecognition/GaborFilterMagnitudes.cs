using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    class GaborFilterMagnitudes
    {
        Bitmap _bmpAfterGabor;

        public GaborFilterMagnitudes(Bitmap bmpAfterGabor)
        {
            _bmpAfterGabor = bmpAfterGabor;
        }

        public List<double> CalculateFeatureVector()
        {
            List<double> fv = new List<double>();

            double mean = CalculateMean();
            double std = CalculateStd(mean);
            double skew = CalculateSkew(mean, std);

            fv.Add(mean);
            fv.Add(std);
            fv.Add(skew);

            return fv;
        }

        public double CalculateMean()
        {
            double mean = 0;

            for(int i = 0; i < _bmpAfterGabor.Width; i++)
            {
                for(int j = 0; j < _bmpAfterGabor.Height; j++)
                {
                    mean += _bmpAfterGabor.GetPixel(i, j).R;
                }
            }
            mean /= (_bmpAfterGabor.Width * _bmpAfterGabor.Height);
            return mean;
        }

        public double CalculateStd(double mean)
        {
            double std = 0;

            for (int i = 0; i < _bmpAfterGabor.Width; i++)
            {
                for (int j = 0; j < _bmpAfterGabor.Height; j++)
                {
                    std += Math.Pow(Math.Abs(Math.Abs(_bmpAfterGabor.GetPixel(i, j).R) - mean), 2);
                }
            }
            std = Math.Sqrt(std);
            return std;
        }

        public double CalculateSkew(double mean, double std)
        {
            double skew = 0;

            for (int i = 0; i < _bmpAfterGabor.Width; i++)
            {
                for (int j = 0; j < _bmpAfterGabor.Height; j++)
                {
                    skew = Math.Pow((_bmpAfterGabor.GetPixel(i, j).R - mean) / std, 3);
                }
            }
            skew /= (_bmpAfterGabor.Width * _bmpAfterGabor.Height);
            return skew;
        }

    }
}
