using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public class GaborFilterMagnitudes
    {
        private FastBitmap _bmpAfterGabor;
        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="bmpAfterGabor"> bitmap preprocessed by Gabor filter</param>
        public GaborFilterMagnitudes(Bitmap bmpAfterGabor)
        {
            _bmpAfterGabor = new FastBitmap(bmpAfterGabor);
        }

        /// <summary>
        /// Function calculating the feature vectore for the gabor filter results
        /// </summary>
        /// <returns> List of Feature vectors for the bitmap held in the class stored as a list of double</returns>
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
        /// <summary>
        /// Function calculating the mean value of all the pixels in the bitmap
        /// </summary>
        /// <returns>Mean value</returns>
        public double CalculateMean()
        {
            double mean = 0;
            _bmpAfterGabor.Start();

            for(int i = 0; i < _bmpAfterGabor.Width; i++)
            {
                for(int j = 0; j < _bmpAfterGabor.Height; j++)
                {
                    var c = _bmpAfterGabor.GetPixel(i, j);
                    mean += c.R;
                }
            }
            mean /= (_bmpAfterGabor.Width * _bmpAfterGabor.Height);
            _bmpAfterGabor.End();
            return mean;
        }
        /// <summary>
        /// Function calculating the standard deviation for all the pixels in the bitmap
        /// </summary>
        /// <param name="mean"> mean for all the pixels in the bitmap </param>
        /// <returns>standard deviation</returns>
        public double CalculateStd(double mean)
        {
            double std = 0;
            _bmpAfterGabor.Start();
            for (int i = 0; i < _bmpAfterGabor.Width; i++)
            {
                for (int j = 0; j < _bmpAfterGabor.Height; j++)
                {
                    std += Math.Pow(Math.Abs(Math.Abs(_bmpAfterGabor.GetPixel(i, j).R) - mean), 2);
                }
            }
            std = Math.Sqrt(std);
            _bmpAfterGabor.End();
            return std;
        }
        /// <summary>
        /// Function calculating the average skewness for all the pixels in the bitmap
        /// </summary>
        /// <param name="mean">mean value for all the pixels in the bitmap</param>
        /// <param name="std">standard deviation</param>
        /// <returns>skewness</returns>
        public double CalculateSkew(double mean, double std)
        {
            double skew = 0;
            _bmpAfterGabor.Start();
            for (int i = 0; i < _bmpAfterGabor.Width; i++)
            {
                for (int j = 0; j < _bmpAfterGabor.Height; j++)
                {
                    skew += Math.Pow((_bmpAfterGabor.GetPixel(i, j).R - mean) / std, 3);
                }
            }
            skew /= (_bmpAfterGabor.Width * _bmpAfterGabor.Height);
            _bmpAfterGabor.End();
            return skew;
        }

    }
}
