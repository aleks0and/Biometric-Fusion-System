using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceRecognition;

namespace WavFileReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap b = new Bitmap(@"../../im-grayscale.bmp");

            double[] lambda = { 10, 5 };
            GaborFilter gf = new GaborFilter(2, 1, lambda, Math.PI/2, 4, 3);

            List<Bitmap> lb = gf.ApplyFilter(b);

            for(int u=0; u<lb.Count; u++)
            {
                lb[u].Save("img" + u + ".bmp");
            }

            var superpos = new Bitmap(lb[0].Width, lb[0].Height);
            for(int i = 0; i < superpos.Width; i++)
            {
                for(int j = 0; j < superpos.Height; j++)
                {
                    int min = 255;
                    for(int k = 0; k < lb.Count; k++)
                    {
                        var px = lb[k].GetPixel(i, j);
                        if(px.R < min)
                        {
                            min = px.R;
                        }
                    }
                    superpos.SetPixel(i, j, Color.FromArgb(min, min, min));
                }
            }
            superpos.Save("super.bmp");
        }
    }
}
