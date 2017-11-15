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
            Bitmap b = new Bitmap("pict.bmp");

            double[] lambda = {4.0/3, 2.0/3 };
            GaborFilter gf = new GaborFilter(2, 1, lambda, Math.PI/2, 5);

            List<Bitmap> lb = gf.ApplyFilter(b);

            for(int u=0; u<lb.Count; u++)
            {
                lb[u].Save("img" + u + ".bmp");
            }
        }
    }
}
