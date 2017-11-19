using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    class Histogram
    {
        public static void GetHistogram(Bitmap bmp, int[] r, int[] g, int[] b)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var p = bmp.GetPixel(i, j);
                    r[p.R]++;
                    g[p.G]++;
                    b[p.B]++;
                }
            }
        }

    }
}
