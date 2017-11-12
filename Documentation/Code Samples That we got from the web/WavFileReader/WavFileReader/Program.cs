using SpeechRecognition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavFileReader
{
    class Program
    {
        public class WavHeader
        {
            public byte[] riffID; //not needed
            public uint size;  
            public byte[] wavID;  //not needed
            public byte[] fmtID;  //not needed
            public uint fmtSize; 
            public ushort format; //if 1 then uncompressed, anything else -> compressed
            public ushort channels; //mono/stereo
            public uint sampleRate; //https://pl.wikipedia.org/wiki/Pr%C3%B3bkowanie
            public uint bytePerSec; //SampleRate * NumChannels * BitsPerSample/8
            public ushort blockSize; // NumChannels* BitsPerSample/8 = number of bytes per one sample for all channels
            public ushort bit;  
            public byte[] dataID; //not needed
            public uint dataSize; // == NumSamples * NumChannels * BitsPerSample/8 

            public override string ToString()
            {
                Console.WriteLine("RiffId " + riffID[0] + " " +
                    riffID[1] + " " + riffID[2] + " " + riffID[3]);
                Console.WriteLine("Size " + size);
                Console.WriteLine("WavId " + wavID[0] + " " +
                    wavID[1] + " " + wavID[2] + " " + wavID[3]);
                Console.WriteLine("FmtId " + fmtID[0] + " " +
                    fmtID[1] + " " + fmtID[2] + " " + fmtID[3]);
                Console.WriteLine("FmtSize " + fmtSize);
                Console.WriteLine("Format " + format);
                Console.WriteLine("Channels " + channels);
                Console.WriteLine("SampleRate " + sampleRate);
                Console.WriteLine("Bytes per second " + bytePerSec);
                Console.WriteLine("Block size " + blockSize);
                Console.WriteLine("Bit " + bit);
                Console.WriteLine("DataId " + dataID[0] + " " +
                    dataID[1] + " " + dataID[2] + " " + dataID[3]);
                Console.WriteLine("Data size " + dataSize);
                return null;
            }
        }

        static void Main(string[] args)
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
            var frame = frameMaker.ToFrames(lDataList, (int)Header.sampleRate);
            Frame sampleCopy = new Frame(frame[120].Samples);
            Window windowFunction = new GaussWindow();
            
            for (int i = 0; i < frame.Count; i++)
            {
                windowFunction.ApplyWindow(frame[i]);
            }
            Frame sampleCheck = new Frame(frame[120].Samples);
            foreach (var sample in frame)
            {
                foreach (var f in sample.Samples)
                {
                    if (f > 0)
                    {
                        Console.WriteLine("f =" + f);
                    }
                }
            }
            return;
        }
    }
}
