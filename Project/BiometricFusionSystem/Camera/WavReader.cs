using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    public class WavReader
    {
        /// <summary>
        /// Function reads the wav. file from a given path
        /// </summary>
        /// <param name="path"> Path from which the program should get a wav. file</param>
        /// <returns> A WavFile element which holds all the information read from the file</returns>
        public static WavFile Read(string path)
        {
            WavHeader Header = new WavHeader();
            List<short> leftChannel = new List<short>();
            List<short> rightChannel = new List<short>();

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
                        leftChannel.Add((short)br.ReadUInt16());
                        index = i;
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            return new WavFile()
            {
                Header = Header,
                LeftChannel = leftChannel,
                RightChannel = rightChannel
            };
        }
    }
}
