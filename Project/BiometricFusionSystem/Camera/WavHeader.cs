using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
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
}
