using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    class GaborFilter
    {
        private double _stdDevX;
        private double _stdDevY;
        private double[] _lambda;
        private double _phase;
        private int _n;
        private int _kernelSize;

        public GaborFilter(double stdDevX, double stdDevY, double[] lambda, double phase, int n, int kernelSize)
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


        public Bitmap CalculateGaborFilter(Bitmap b, int k, double lambda)
        {
            //CalculateStdDev(b);

            int max = _kernelSize - 1;
            Bitmap bmp = new Bitmap(b.Width - max, b.Height - max);
            var kernel = GetKernel(k, lambda);

            for(int i = max / 2; i < b.Width - max / 2; i++)
            {
                for(int j = max / 2; j < b.Height - max / 2; j++)
                {
                    ApplyKernel(b, bmp, i, j, kernel);
                }
            }
            return bmp;
        }

        public double Clamp(double c)
        {
            if (c > 255)
                return 255;
            else if (c < 0)
                return 0;
            else
                return c;
        }

        public List<Bitmap> ApplyFilter(Bitmap b)
        {
            List<Bitmap> lb = new List<Bitmap>();
            for (int l = 0; l < _lambda.Count(); l++)
            {
                for (int k = 1; k <= _n; k++)
                {
                    lb.Add(CalculateGaborFilter(b, k, _lambda[l]));
                }
            }
            return lb;
        }

        private double GetGaborValue(int x, int y, int angle, double waveLength)
        {
            double xTheta = x * Math.Cos(angle * Math.PI / _n) + y * Math.Sin(angle * Math.PI / _n);
            double yTheta = y * Math.Cos(angle * Math.PI / _n) - x * Math.Sin(angle * Math.PI / _n);
            double exp = -(Math.Pow(xTheta, 2) / Math.Pow(_stdDevX, 2) + Math.Pow(yTheta, 2) / Math.Pow(_stdDevY, 2));
            double cos = 2 * Math.PI * (xTheta / waveLength) + _phase;
            return Math.Exp(exp) * Math.Cos(cos);
        }

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

        private void ApplyKernel(Bitmap src, Bitmap dest, int x, int y, double[,] kernel)
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
