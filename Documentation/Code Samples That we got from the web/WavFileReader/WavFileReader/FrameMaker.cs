using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition
{
    public class FrameMaker
    {
        private float _frameLength;
        private float _frameInterval;
        public FrameMaker(float frameLength, float frameInterval)
        {
            _frameLength = frameLength;
            _frameInterval = frameInterval;
        }
        /* frames should be of length ~50ms and intersect each other within 25ms-30ms 
          number of samples per frame = length of frame in seconds * samplerate(or bps) */
        public List<Frame> ToFrames(List<short> audioStream, int sampleRate)
        {
            int samplesPerFrame = (int)(_frameLength * sampleRate);
            int step = (int)(_frameInterval * sampleRate);
            var frames = new List<Frame>();

            int index = 0;
            while(index + step < audioStream.Count)
            {
                if (index + samplesPerFrame < audioStream.Count)
                {
                    frames.Add(new Frame(audioStream.GetRange(index, samplesPerFrame)));
                }
                index += step;
                
            }

            return frames;
        }
    }
}
