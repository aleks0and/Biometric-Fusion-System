using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    public class WavHeader
    {
        public byte[] riffID; //"RIFF" marks the file as riff file.
        public uint size;     //Size of overall file, in bytes (32-bit integer)
        public byte[] wavID;  //"WAVE" file type header
        public byte[] fmtID;  //"fmt" Format chunk marker
        public uint fmtSize;  //Length of format data
        public ushort format; //Type of format - if 1 then uncompressed(PCM), anything else -> compressed
        public ushort channels; //number of channels - 1 for mono
        public uint sampleRate; //Sample Rate - Number of Samples per second, or Hertz 32 byte integer. Common values are 44100 (CD), 48000 (DAT).  https://pl.wikipedia.org/wiki/Pr%C3%B3bkowanie
        public uint bytePerSec; //(SampleRate * NumChannels * BitsPerSample)/8
        public ushort blockSize; // NumChannels* BitsPerSample/8 = number of bytes per one sample for all channels  
        public ushort bit;      //Bits per sample
        public byte[] dataID; //"data" marks the beginning of the data section
        public uint dataSize; // == NumSamples * NumChannels * BitsPerSample/8 
        /// <summary>
        /// Function writes onto console all the header information.
        /// </summary>
        /// <returns></returns>
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
