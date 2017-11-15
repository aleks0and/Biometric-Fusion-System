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
        private double stdDevX;
        private double stdDevY;
        private double[] lambda;
        private double phase;
        private int n;

        public GaborFilter(double _stdDevX, double _stdDevY, double[] _lambda, double _phase, int _n)
        {
            stdDevX = _stdDevX;
            stdDevY = _stdDevY;
            lambda = _lambda;
            phase = _phase;
            n = _n;
        }


        public Bitmap CalculateGaborFilter(Bitmap b, int k, double lambda)
        {
            Bitmap bmp = new Bitmap(b.Width, b.Height);
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    double xThetak = i * Math.Cos(k*Math.PI/n) + j * Math.Sin(k * Math.PI / n);
                    double yThetak = j * Math.Cos(k * Math.PI / n) - i * Math.Sin(k * Math.PI / n);

                    double exp = -1 * (Math.Pow(xThetak, 2) / Math.Pow(stdDevX, 2) + Math.Pow(yThetak, 2) / Math.Pow(stdDevY, 2));
                    double cos = 2 * Math.PI * 1 / lambda + phase;
                    double gabor = Math.Exp(exp) * Math.Cos(cos);
                    Color c = Color.FromArgb((int)gabor);
                    bmp.SetPixel(i, j, c);
                }
            }
            return bmp;
        }

        public List<Bitmap> ApplyFilter(Bitmap b)
        {
            List<Bitmap> lb = new List<Bitmap>();
            for (int l = 0; l < lambda.Count(); l++)
            {
                for (int k = 1; k <= n; k++)
                {
                    lb.Add(CalculateGaborFilter(b, k, l));
                }
            }
            return lb;
        }

    }
}
