using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    public class GrayscaleConverter : INormalization
    {
        /// <summary>
        /// function converting the bitmap into grayscale values
        /// </summary>
        /// <param name="bmp">source bitmap </param>
        /// <returns> grayscale version of original bitmap</returns>
        public Bitmap Normalize (Bitmap bmp)
        {
            FastBitmap b = new FastBitmap(bmp);
            b.Start();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var p = b.GetPixel(i, j);
                    int sum = p.R + p.G + p.B;
                    sum /= 3;
                    b.SetPixel(i, j, Color.FromArgb(sum, sum, sum));
                }
            }
            b.End();
            return bmp;
        }
    }
}
