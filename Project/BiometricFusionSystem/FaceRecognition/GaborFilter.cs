using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    /// <summary>
    /// Class calculating the gabor filter
    /// </summary>
    public class GaborFilter
    {
        private double _stdDevX;
        private double _stdDevY;
        private double _lambda;
        private double _phase;
        private int _n;
        private int _kernelSize;
        /// <summary>
        /// Constructor for the class, setting the initial values for the filter.
        /// </summary>
        /// <param name="stdDevX"> Standard deviation of the Gausian envelope along x </param>
        /// <param name="stdDevY"> Standard deviation of the Gausian envelope along x </param>
        /// <param name="lambda"> wavelength used for the calculation of GF</param>
        /// <param name="phase"> phase for the calculation of orientation</param>
        /// <param name="n"> maximal number of orientations</param>
        /// <param name="kernelSize"> kernel size</param>
        public GaborFilter(double stdDevX, double stdDevY, double lambda, double phase, int n, int kernelSize)
        {
            _stdDevX = stdDevX;
            _stdDevY = stdDevY;
            _lambda = lambda;
            _phase = phase;
            _n = n;
            _kernelSize = kernelSize;
        }

        //private void CalculateStdDev(Bitmap b)
        //{

        //}
        /// <summary>
        /// function which applies the gabor filter kernel to the bitmap 
        /// </summary>
        /// <param name="b"> bitmap upon the filter will be applied</param>
        /// <param name="k"> current angle for the orientation</param>
        /// <param name="lambda"> wavelength for which filter is calculated</param>
        /// <returns> bitmap after the aplication of the filter </returns>
        public Bitmap CalculateGaborFilter(Bitmap b, int k, double lambda)
        {
            //CalculateStdDev(b);
            FastBitmap src = new FastBitmap(b);
            int max = _kernelSize - 1;
            FastBitmap dest = new FastBitmap(new Bitmap(b.Width - max, b.Height - max));
            var kernel = GetKernel(k, lambda);

            src.Start();
            dest.Start();

            for(int i = max / 2; i < b.Width - max / 2; i++)
            {
                for(int j = max / 2; j < b.Height - max / 2; j++)
                {
                    ApplyKernel(src, dest, i, j, kernel);
                }
            }
            src.End();
            dest.End();
            return dest.Bmp;
        }
        /// <summary>
        /// function limiting the value to the max of 255 and min to 0
        /// </summary>
        /// <param name="c"> color value</param>
        /// <returns> color value not exceeding 255 and not less than 0</returns>
        public double Clamp(double c)
        {
            if (c > 255)
                return 255;
            else if (c < 0)
                return 0;
            else
                return c;
        }
        /// <summary>
        /// function applying the gabor filter with all combinations of angles and wavelenghth
        /// </summary>
        /// <param name="b"> initial bitmap</param>
        /// <returns> list of bitmaps with applied gabor filter (with different wavelengths and angles) </returns>
        public List<Bitmap> ApplyFilter(Bitmap b)
        {
            List<Bitmap> lb = new List<Bitmap>();
            for (int k = 1; k <= _n; k++)
            {
                lb.Add(CalculateGaborFilter(b, k, _lambda));
            }
            return lb;
        }
        /// <summary>
        /// Function calculating the mathematical formula for gabor filter.
        /// </summary>
        /// <param name="x"> horizontal coordinate</param>
        /// <param name="y">  vertical coordinate</param>
        /// <param name="angle"> angle at which the value is calculated</param>
        /// <param name="waveLength"> lambda currently used for calculation</param>
        /// <returns> value of gabor filter for single pixel </returns>
        public double GetGaborValue(int x, int y, int angle, double waveLength)
        {
            double xTheta = x * Math.Cos(angle * Math.PI / _n) + y * Math.Sin(angle * Math.PI / _n);
            double yTheta = y * Math.Cos(angle * Math.PI / _n) - x * Math.Sin(angle * Math.PI / _n);
            double exp = -(Math.Pow(xTheta, 2) / Math.Pow(_stdDevX, 2) + Math.Pow(yTheta, 2) / Math.Pow(_stdDevY, 2));
            double cos = 2 * Math.PI * (xTheta / waveLength) + _phase;
            return Math.Exp(exp) * Math.Cos(cos);
        }

        /// <summary>
        /// function creating kernels with gabor filter values. 
        /// </summary>
        /// <param name="angle">angle at which the value is calculated</param>
        /// <param name="waveLength"> lambda currently used for calculation</param>
        /// <returns> kernel with the computed gabor filter values</returns>
        private double[,] GetKernel(int angle, double waveLength)
        {
            int max = (_kernelSize - 1) / 2;
            var kernel = new double[_kernelSize, _kernelSize];
            double sum = 0;
            for(int x = -max; x <= max; x++)
            {
                for(int y = -max; y <= max; y++)
                {
                    kernel[x + max, y + max] = GetGaborValue(x, y, angle, waveLength);
                    sum += kernel[x + max, y + max];
                }
            }
            return kernel;
        }
        /// <summary>
        /// function applying the gabor filter kernel to the bitmap
        /// </summary>
        /// <param name="src"> source bitmap</param>
        /// <param name="dest"> bitmap after the application of gabor filter kernel</param>
        /// <param name="x"> horizontal coordinates of pixel</param>
        /// <param name="y"> vertical coordinates of pixel</param>
        /// <param name="kernel"> kernel with current gabor filter values</param>
        private void ApplyKernel(FastBitmap src, FastBitmap dest, int x, int y, double[,] kernel)
        {
            int max = (_kernelSize - 1) / 2;
            double sumR = 0, sumG = 0, sumB = 0;
            for (int i = -max; i <= max; i++)
            {
                for (int j = -max; j <= max; j++)
                {
                    var srcColor = src.GetPixel(x + i, y + j);
                    sumR += kernel[i + max, j + max] * srcColor.R;
                    sumG += kernel[i + max, j + max] * srcColor.G;
                    sumB += kernel[i + max, j + max] * srcColor.B;
                }
            }
            dest.SetPixel(x - max, y - max, Color.FromArgb(255 - (int)Clamp(sumR), 255 - (int)Clamp(sumG), 255 - (int)Clamp(sumB)));
        }
    }
}
