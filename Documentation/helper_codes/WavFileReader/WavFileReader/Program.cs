using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceRecognition;
using System.Drawing.Imaging;
using Common;


namespace WavFileReader
{
    class Program
    {
        static void Main(string[] args)
        {
           // DbTest();
            GaborTest();
        }
        private static void DbTest()
        {
            //face
            //Bitmap b = new Bitmap(@"../../im-grayscale.bmp");
            //byte[] img = b.ToByteArray(ImageFormat.Bmp);

            Common.DbConnection dbc = new Common.DbConnection();

            //Common.FaceRepository fr = new Common.FaceRepository(dbc);

            //List<double> fv = new List<double>{ 1.0, 2.0, 3.0 };

            //string strfv = "1.0,1.1,1.2,1.3";

            //fr.SaveFace(img, strfv, "first", "last");


            //voice
            byte[] speech = { 0, 1, 2, 3};
            string path = "../../Samples/Aleks_kurczak.wav";
            List<double> periodogramResult = PeriodogramTest(path);
            string featureVector = Person.FeatureVectorToString(periodogramResult);
            Common.SpeechRepository sr = new Common.SpeechRepository(dbc);

            //sr.SaveSpeech(speech, featureVector, "Aleks", "Kurczak?");

            //porownanie Aleks do Aleks
            string path2 = "../../Samples/Aleks_kurczak2.wav";
            List<double> periodogramResult2 = PeriodogramTest(path2);

            DynamicTimeWarping dtw = new DynamicTimeWarping(0.1);
            double answer = dtw.Compare(periodogramResult2, periodogramResult);
            Console.WriteLine("ANSWER Aleks=Aleks: " + answer);

            //porownanie Aleks do Martyna
            string path3 = "../../Samples/martyna_kurczak.wav";
            List<double> periodogramResult3 = PeriodogramTest(path3);
            
            double answer3 = dtw.Compare(periodogramResult3, periodogramResult);
            Console.WriteLine("ANSWER Aleks=Martyna: " + answer3);

            //porownanie Aleks do Kornel
            string path4 = "../../Samples/kornel_kurczak.wav";
            List<double> periodogramResult4 = PeriodogramTest(path4);
            
            double answer4 = dtw.Compare(periodogramResult4, periodogramResult);
            Console.WriteLine("ANSWER Aleks=Kornel: " + answer4);
            Console.WriteLine("Speech test finished");
            Console.Read();
        }
        private static List<double> PeriodogramTest(string path)
        {
            WavHeader Header = new WavHeader();
            List<short> lDataList = new List<short>();
            List<short> rDataList = new List<short>();
            
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

            FrameMaker frameMaker = new FrameMaker(frameLength: 0.05f, frameInterval: 0.025f);
            var frames = frameMaker.ToFrames(lDataList, (int)Header.sampleRate);
            Window windowFunction = new HammingWindow();

            for (int i = 0; i < frames.Count; i++)
            {
                windowFunction.ApplyWindow(frames[i]);
            }
            var estimates = new List<List<double>>();
            foreach(var frame in frames)
            {
                var estimate = Periodogram.GetEstimate(frame);
                estimates.Add(estimate);
            }

            MelFilterbank melFilterbank = new MelFilterbank(
                lowerFreq: 300, 
                upperFreq: 22050, 
                filterbanksCount: 10, 
                samplerate: (int)Header.sampleRate, 
                fourierLength: estimates[0].Count);

            melFilterbank.GenerateFilterbankIntervals();
            melFilterbank.ConvertFilterbanks();
            melFilterbank.CalculateFilters();
            var fbs = melFilterbank.CreateFilterbanks();
            List<List<double>> dct = new List<List<double>>();
            for(int k = 0; k < estimates.Count; k++)
            {
                melFilterbank.CalculateFilterbanksEnergies(estimates[k], fbs);
                dct.Add(melFilterbank.DiscreteCosineTransform());
            }
            var result = dct.SelectMany(n => n).ToList();
            return result; 
        }
    
        private static void GaborTest()
        {
            Bitmap b = new Bitmap(@"../../Aleks1.bmp");

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

    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }

}


