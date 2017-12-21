using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;
using Camera;
using System.Windows;
using System.Threading;
using SpeechRecognition;

namespace Gui.Model
{
    public class PersonData : BindableBase
    {
        private BitmapImage _image;
        private uint _sampleRate;
        private List<short> _samples;
        private string _imagePath;
        private string _imageSize;
        private string _recordingLength;
        private string _recordingPath;
        private Uri _recordingUri;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; Notify(); }
        } 
        public uint SampleRate
        {
            get { return _sampleRate; }
        }
        public List<short> Samples
        {
            get { return _samples; }
            set { _samples = value; Notify(); }
        }
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; Notify(); }
        }
        public string ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; Notify(); }
        }
        public string RecordingLength
        {
            get { return _recordingLength; }
            set { _recordingLength = value; Notify(); }
        }
        public string RecordingPath
        {
            get { return _recordingPath; }
            set { _recordingPath = value; Notify(); }
        }
        public Uri RecordingUri
        {
            get { return _recordingUri; }
            set { _recordingUri = value; Notify(); }
        }
        public void LoadImage(string path, AcquisitionMethod acquisitionMethod)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(() =>
                {
                    if(acquisitionMethod == AcquisitionMethod.FromCamera)
                    { 
                        path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + path;
                    }
                    Image = new BitmapImage(new Uri(path));
                    ImagePath = "Path: " + path;
                    ImageSize = "Image width: " + (int)Image.Width + ", height: " + (int)Image.Height;
                }));
        }

        public void LoadWavFile(string path, AcquisitionMethod acquisitionMethod)
        {
            if (acquisitionMethod == AcquisitionMethod.FromCamera)
            {
                path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + path;
            }
            //var wavFile = WavReader.Read(@"bin\" + path);
            var wavFile = WavReader.Read(path);
            _sampleRate = wavFile.Header.sampleRate;
            Samples = wavFile.LeftChannel;
            RecordingPath = "Path: " + path;
            RecordingUri = new Uri(path);
            double seconds = Samples.Count / (double)SampleRate;
            double milliseconds = (seconds - Math.Floor(seconds)) * 1000;
            var time = new DateTime(1, 1, 1, 0, 0, (int)seconds, (int)milliseconds);

            RecordingLength = "Length: " + time.ToString("mm:ss:fff");
        }

        public void RemoveSilence(Model.SilenceRemoval silenceRemoval)
        {
            if (silenceRemoval == Model.SilenceRemoval.Yes)
            {
                SpeechRecognition.SilenceRemoval removal = new SpeechRecognition.SilenceRemoval(50, 5);
                Samples = removal.RemoveSilence(Samples);
            }
        }

        public void NormalizeVolume()
        {
            VolumeNormalizer norm = new VolumeNormalizer(0, 1000);
            Samples = norm.Normalize(Samples);
        }
    }
}
