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

namespace Gui.Model
{
    public class PersonData : BindableBase
    {
        private BitmapImage _image;
        private uint _sampleRate;
        private List<short> _samples;
        private string _imagePath;
        private string _imageSize;

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

        public void LoadImage(string path)
        {
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + path;
            Image = new BitmapImage(new Uri(path));
            ImagePath = "Path: " + path;
            ImageSize = "Image width: " + (int)Image.Width + ", height: " + (int)Image.Height;
        }

        public void LoadWavFile(string path)
        {
            var wavFile = WavReader.Read(path);
            _sampleRate = wavFile.Header.sampleRate;
            _samples = wavFile.LeftChannel;
        }
    }
}
