using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public class Histogram
    {
        /// <summary>
        /// function calculating the color histogram for a given bitmap
        /// </summary>
        /// <param name="bmp">source bitmap</param>
        /// <param name="r">array representing the occurences red channel values</param>
        /// <param name="g">array representing the occurences green channel values</param>
        /// <param name="b">array representing the occurences blue channel values</param>
        public static void GetHistogram(Bitmap bmp, int[] r, int[] g, int[] b)
        {
            FastBitmap fastBmp = new FastBitmap(bmp);
            fastBmp.Start();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var p = fastBmp.GetPixel(i, j);
                    r[p.R]++;
                    g[p.G]++;
                    b[p.B]++;
                }
            }
            fastBmp.End();
        }

    }
}
