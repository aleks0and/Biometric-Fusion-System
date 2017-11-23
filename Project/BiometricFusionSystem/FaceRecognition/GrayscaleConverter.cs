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
        public Bitmap Normalize (Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var p = bmp.GetPixel(i, j);
                    int sum = p.R + p.G + p.B;
                    sum /= 3;
                    bmp.SetPixel(i, j, Color.FromArgb(sum, sum, sum));
                }
            }
            return bmp;
        }
    }
}
