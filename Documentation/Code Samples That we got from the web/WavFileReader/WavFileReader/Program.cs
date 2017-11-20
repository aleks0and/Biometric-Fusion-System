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
            PeriodogramTest();
        }
        private static void DbTest()
        {

        }
        private static void PeriodogramTest()
        {
            WavHeader Header = new WavHeader();
            List<short> lDataList = new List<short>();
            List<short> rDataList = new List<short>();
            string path = "../../Samples/0_0_0_0_1_1_1_1.wav";
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                try
                {
                    Header.riffID = br.ReadBytes(4);
                    Header.size = br.ReadUInt32();
                    Header.wavID = br.ReadBytes(4);
                    Header.fmtID = br.ReadBytes(4);
                    Header.fmtSize = br.ReadUInt32();
                    Header.format = br.ReadUInt16();
                    Header.channels = br.ReadUInt16();
                    Header.sampleRate = br.ReadUInt32();
                    Header.bytePerSec = br.ReadUInt32();
                    Header.blockSize = br.ReadUInt16();
                    Header.bit = br.ReadUInt16();
                    Header.dataID = br.ReadBytes(4);
                    Header.dataSize = br.ReadUInt32();
                    int index = 0;
                    //1 channel --> add to 1 list, 2 channels --> add to 2 lists
                    for (int i = 0; i < Header.dataSize / Header.blockSize; i++)
                    {
                        lDataList.Add((short)br.ReadUInt16());
                        index = i;
                    }
                }
                finally
                {
                    if (br != null)
                    {
                        br.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            Console.WriteLine(Header);

            FrameMaker frameMaker = new FrameMaker(frameLength: 0.05f, frameInterval: 0.025f);
            var frames = frameMaker.ToFrames(lDataList, (int)Header.sampleRate);
            Frame sampleCopy = new Frame(frames[120].Samples);
            Window windowFunction = new GaussWindow();

            for (int i = 0; i < frames.Count; i++)
            {
                windowFunction.ApplyWindow(frames[i]);
            }
            Frame sampleCheck = new Frame(frames[120].Samples);
            foreach (var sample in frames)
            {
                foreach (var f in sample.Samples)
                {
                    if (f > 0)
                    {
                        Console.WriteLine("f =" + f);
                    }
                }
            }
            var periodogram = new Periodogram();
            var estimates = new List<List<double>>();
            foreach(var frame in frames)
            {
                var estimate = periodogram.GetEstimate(frame);
                estimates.Add(estimate);
            }

            MelFilterbank melFilterbank = new MelFilterbank(300, 8000, 10, (int)Header.sampleRate, estimates[0].Count);

            melFilterbank.GenerateFilterbanks();
            melFilterbank.ConvertFilterbanks();
            melFilterbank.CalculateFilters();
            var fbs = melFilterbank.CreateFilterbanks();
            List<List<double>> dct = new List<List<double>>();
            for(int k = 0; k < estimates.Count; k++)
            {
                melFilterbank.CalculateFilterbanksEnergies(estimates[k], fbs);
                dct.Add(melFilterbank.DiscreteCosineTransform());
            }

            Console.WriteLine("THE END------------------------------------");
        }
    
        private static void GaborTest()
        {
            Bitmap b = new Bitmap(@"../../im-grayscale.bmp");

            double[] lambda = { 10, 5 };
            GaborFilter gf = new GaborFilter(2, 1, lambda, Math.PI / 2, 4, 3);

            List<Bitmap> lb = gf.ApplyFilter(b);

            for (int u = 0; u < lb.Count; u++)
            {
                lb[u].Save("img" + u + ".bmp");
            }

            var superpos = new Bitmap(lb[0].Width, lb[0].Height);
            for (int i = 0; i < superpos.Width; i++)
            {
                for (int j = 0; j < superpos.Height; j++)
                {
                    int min = 255;
                    for (int k = 0; k < lb.Count; k++)
                    {
                        var px = lb[k].GetPixel(i, j);
                        if (px.R < min)
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
