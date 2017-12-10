﻿using Gui.Utility;
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

        public void LoadImage(string path)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                new Action(() =>
                {
                    //path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + path;
                    Image = new BitmapImage(new Uri(path));
                    ImagePath = "Path: " + path;
                    ImageSize = "Image width: " + (int)Image.Width + ", height: " + (int)Image.Height;
                }));
        }

        public void LoadWavFile(string path)
        {
            //var wavFile = WavReader.Read(@"bin\" + path);
            var wavFile = WavReader.Read(path);
            _sampleRate = wavFile.Header.sampleRate;
            _samples = wavFile.LeftChannel;
            RecordingPath = "Path: " + path;
            double seconds = Samples.Count / (double)SampleRate;
            double milliseconds = (seconds - Math.Floor(seconds)) * 1000;
            var time = new DateTime(1, 1, 1, 0, 0, (int)seconds, (int)milliseconds);

            RecordingLength = "Length: " + time.ToString("mm:ss:fff");
        }
    }
}
