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
        int _height;
        int _width;
        private FastBitmap _bmp;

        /// <summary>
        /// Constructor for the class setting the initial values
        /// </summary>
        /// <param name="width"> width of the bitmap</param>
        /// <param name="height"> height of the bitmap</param>
        /// <param name="bmp"> initial bitmap </param>
        public HistogramFeatureExtraction(Bitmap bmp)
        {
            _height = bmp.Height;
            _width = bmp.Width;
            _bmp = new FastBitmap(bmp);
        }

        // second Moment is the Variance, third is the Skewness
        /// <summary>
        /// Function calculating the moments which are to be used as feature vector
        /// </summary>
        /// <param name="finalMoment"> the iteration at which the calculation of moments stops</param>
        /// <returns> feature vector based on the color histogram </returns>
        public List<List<double>> CalculateMoments (int finalMoment)
        {
            _bmp.Start();
            List<List<double>> moments = new List<List<double>>();
            moments.Add(CalculateMean());
            for (int i = 2; i <= finalMoment; i++)
            {
                moments.Add(CalculateIthMoment(i, moments[0]));
            }
            _bmp.End();
            return moments;
            
        }
        /// <summary>
        /// Function calculating the first moment for the color histogram which is equal to the mean value of the colors.
        /// </summary>
        /// <returns> Mean value for each color channel stored as a list of double </returns>
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

        /// <summary>
        /// Function calculating the moments starting at 2, it requires the calculation of the first moment beforehand
        /// 2nd moment is the variance for the color channel, 3rd is the skewness
        /// </summary>
        /// <param name="moment"> moment number which is to be calculated</param>
        /// <param name="mean"> mean values for the color channels necessary for the calculation</param>
        /// <returns> values for the ith moment stored as list of double</returns>
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
                    sumR += Math.Pow(Math.Abs(_bmp.GetPixel(i, j).R - mean[0]), moment);
                    sumG += Math.Pow(Math.Abs(_bmp.GetPixel(i, j).G - mean[1]), moment);
                    sumB += Math.Pow(Math.Abs(_bmp.GetPixel(i, j).B - mean[2]), moment);
                }
            }
            int pixelCount = _height * _width;
            sumR /= (double)pixelCount;
            sumG /= (double)pixelCount;
            sumB /= (double)pixelCount;
            sumR = Math.Pow(sumR, 1.0 / moment);
            sumG = Math.Pow(sumG, 1.0 / moment);
            sumB = Math.Pow(sumB, 1.0 / moment);
            ithMoment.Add(sumR);
            ithMoment.Add(sumG);
            ithMoment.Add(sumB);
            return ithMoment;
        }

    }
}
