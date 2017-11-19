using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public class HistogramEqualization : INormalization
    {
        private const int Max = 256;
        public Bitmap Normalize(Bitmap bmp)
        {
            var pmfR = new int[Max];
            var pmfG = new int[Max];
            var pmfB = new int[Max];
            Histogram.GetHistogram(bmp, pmfR, pmfG, pmfB);

            var cdfR = new double[Max];
            var cdfG = new double[Max];
            var cdfB = new double[Max];
            FindCdf(bmp, pmfR, pmfG, pmfB, cdfR, cdfG, cdfB);

            int[] channelMax = GetChannelMax(bmp);
            int[] channelMin = GetChannelMin(bmp);
            int[,] equalizedColors = GetEqualizedColors(cdfR, cdfG, cdfB, channelMax, channelMin);

            return Equalize(bmp, equalizedColors);
        }



        private void FindCdf(Bitmap bmp, int[] pmfR, int[] pmfG, int[] pmfB,
            double[] cdfR, double[] cdfG, double[] cdfB)
        {
            int maxSize = bmp.Width * bmp.Height;
            for(int i = 0; i < Max; i++)
            {
                int j = i;
                while(j >= 0)
                {
                    cdfR[i] += pmfR[j];
                    cdfG[i] += pmfG[j];
                    cdfB[i] += pmfB[j];
                    j--;
                }
                cdfR[i] /= maxSize;
                cdfG[i] /= maxSize;
                cdfB[i] /= maxSize;
            }
        }

        private int[] GetChannelMax(Bitmap bmp)
        {
            int[] channels = new int[3];

            for(int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {
                    var p = bmp.GetPixel(i, j);
                    if(p.R > channels[0])
                    {
                        channels[0] = p.R;
                    }
                    if(p.G > channels[1])
                    {
                        channels[1] = p.G;
                    }
                    if(p.B > channels[2])
                    {
                        channels[2] = p.B;
                    }
                }
            }
            return channels;
        }
        private int[] GetChannelMin(Bitmap bmp)
        {
            int[] channels = new int[3] { 255, 255, 255 };
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var p = bmp.GetPixel(i, j);
                    if (p.R < channels[0])
                    {
                        channels[0] = p.R;
                    }
                    if (p.G < channels[1])
                    {
                        channels[1] = p.G;
                    }
                    if (p.B < channels[2])
                    {
                        channels[2] = p.B;
                    }
                }
            }
            return channels;
        }

        private int[,] GetEqualizedColors(double[] cdfR, double[] cdfG, double[] cdfB,
            int[] channelMax, int[] channelMin)
        {
            int[,] equalizedColors = new int[3, Max];
            for(int i = 0; i < Max; i++)
            {
                equalizedColors[0, i] = (int)Math.Floor(cdfR[i] * (channelMax[0] - channelMin[0]) + channelMin[0]);
                equalizedColors[1, i] = (int)Math.Floor(cdfR[i] * (channelMax[1] - channelMin[1]) + channelMin[1]);
                equalizedColors[2, i] = (int)Math.Floor(cdfR[i] * (channelMax[2] - channelMin[2]) + channelMin[2]);
            }

            return equalizedColors;
        }

        private Bitmap Equalize(Bitmap bmp, int[,] equalizedColors)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            for(int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {
                    var p = bmp.GetPixel(i, j);
                    newBmp.SetPixel(i, j,
                        Color.FromArgb(
                            equalizedColors[0, p.R],
                            equalizedColors[1, p.G],
                            equalizedColors[2, p.B]));
                }
            }
            return newBmp;
        }
    }
}
