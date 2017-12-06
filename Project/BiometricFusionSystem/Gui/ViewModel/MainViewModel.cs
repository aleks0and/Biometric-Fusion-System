﻿using Camera;
using Gui.Model;
using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gui.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private Ffmpeg _ffmpeg;
        private WindowService _service = new WindowService();
        public ICommand OpenOptionsCommand { get; set; }
        public ICommand OpenVerificationCommand { get; set; }
        public ICommand AcquirePhotoCommand { get; set; }
        public ICommand AcquireRecordingCommand { get; set; }
        
        public MainViewModel()
        {
            OpenOptionsCommand = new RelayCommand(OpenOptions, canExecute => true);
            OpenVerificationCommand = new RelayCommand(OpenVerification, canExecute => true);
            AcquirePhotoCommand = new RelayCommand(AcquirePhoto, canExecute => true);
            AcquireRecordingCommand = new RelayCommand(AcquireRecording, canExecute => true);
            _ffmpeg = new Ffmpeg();
        }

        private void AcquirePhoto(object parameter)
        {
            _ffmpeg.TakePicture("output.bmp", new EventHandler(AcquirePhotoHandler));
        }

        private void AcquireRecording(object parameter)
        {
            _ffmpeg.RecordAudio("output.wav", new EventHandler(AcquireRecordingHandler));
        }

        private void AcquirePhotoHandler(object parameter, EventArgs e)
        { 
            _ffmpeg.EndEvent();
            MessageBox.Show("Photo acquired");

        }
        private void AcquireRecordingHandler(object parameter, EventArgs e)
        {
            _ffmpeg.EndEvent();
            MessageBox.Show("Recording acquired");
        }

        private void OpenOptions(object parameter)
        {
            _service.OpenOptions(this);
        }
        private void OpenVerification(object parameter)
        {
            _service.OpenVerifiaction(this);
        }
    }
}