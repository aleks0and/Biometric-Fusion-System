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
        /// <summary>
        /// Function which applies the histogram equalization to the bitmap
        /// </summary>
        /// <param name="bmp">input bitmap</param>
        /// <returns>bitmap with applied histogram equalization</returns>
        public Bitmap Normalize(Bitmap bmp)
        {

            var pmfR = new int[Max];
            var pmfG = new int[Max];
            var pmfB = new int[Max];
            Histogram.GetHistogram(bmp, pmfR, pmfG, pmfB);

            var cdfR = new double[Max];
            var cdfG = new double[Max];
            var cdfB = new double[Max];
            FindCdf(bmp.Width, bmp.Height, pmfR, pmfG, pmfB, cdfR, cdfG, cdfB);

            int[] channelMax = GetChannelMax(bmp);
            int[] channelMin = GetChannelMin(bmp);
            int[,] equalizedColors = GetEqualizedColors(cdfR, cdfG, cdfB, channelMax, channelMin);

            return Equalize(bmp, equalizedColors);
        }


        /// <summary>
        /// Function which calculated cumulative distribution function for all channels of the picture
        /// </summary>
        /// <param name="width">width of the picture</param>
        /// <param name="height">height of the picture</param>
        /// <param name="pmfR"> histogram of red channel</param>
        /// <param name="pmfG"> histogram of green channel</param>
        /// <param name="pmfB"> histogram of blue channel</param>
        /// <param name="cdfR"> cumulative distribution for red channel</param>
        /// <param name="cdfG"> cumulative distribution for green channel</param>
        /// <param name="cdfB"> cumulative distribution for blue channel</param>
        private void FindCdf(int width, int height, int[] pmfR, int[] pmfG, int[] pmfB,
            double[] cdfR, double[] cdfG, double[] cdfB)
        {
            int maxSize = width * height;
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
        /// <summary>
        /// Get the maximum value for all the channels
        /// </summary>
        /// <param name="bmp">initial bitmap</param>
        /// <returns>an array with the max values for all three channels index: 0 for red, 1 for green, 2 for blue</returns>
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
        /// <summary>
        /// Get the maximum value for all the channels
        /// </summary>
        /// <param name="bmp">initial bitmap</param>
        /// <returns>an array with the min values for all three channels index: 0 for red, 1 for green, 2 for blue</returns>
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
        /// <summary>
        /// Function applies the mathematical formula to calculate the equalized colors for the cumulative distribution function.
        /// </summary>
        /// <param name="cdfR"> cumulative distribution for red channel</param>
        /// <param name="cdfG"> cumulative distribution for green channel</param>
        /// <param name="cdfB"> cumulative distribution for blue channel</param>
        /// <param name="channelMax"> maximum value for all the color channels</param>
        /// <param name="channelMin"> minimum value for all the color channels</param>
        /// <returns> array containing the equalized colors for color channels</returns>
        private int[,] GetEqualizedColors(double[] cdfR, double[] cdfG, double[] cdfB,
            int[] channelMax, int[] channelMin)
        {
            int[,] equalizedColors = new int[3, Max];
            for(int i = 0; i < Max; i++)
            {
                equalizedColors[0, i] = (int)Math.Floor(cdfR[i] * (channelMax[0] - channelMin[0]) + channelMin[0]);
                equalizedColors[1, i] = (int)Math.Floor(cdfG[i] * (channelMax[1] - channelMin[1]) + channelMin[1]);
                equalizedColors[2, i] = (int)Math.Floor(cdfB[i] * (channelMax[2] - channelMin[2]) + channelMin[2]);
            }

            return equalizedColors;
        }
        /// <summary>
        /// function equalizing the colors in the bitmap 
        /// </summary>
        /// <param name="bmp"> initial bitmap</param>
        /// <param name="equalizedColors"> array holding the equalized values for color channels</param>
        /// <returns> bitmap with equalized colors</returns>
        private Bitmap Equalize(Bitmap bmp, int[,] equalizedColors)
        {
            FastBitmap newBmp = new FastBitmap(new Bitmap(bmp.Width, bmp.Height));
            newBmp.Start();
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
            newBmp.End();
            return newBmp.Bmp;
        }
    }
}
