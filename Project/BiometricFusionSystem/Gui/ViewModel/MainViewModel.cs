﻿using Camera;
using Common;
using Gui.Model;
using Gui.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gui.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private Verification _verification;
        private Identification _identification;
        private Ffmpeg _ffmpeg;
        public ICommand OpenOptionsCommand { get; set; }
        public ICommand OpenVerificationCommand { get; set; }
        public ICommand AcquirePhotoCommand { get; set; }
        public ICommand AcquireRecordingCommand { get; set; }
        public ICommand IdentifyCommand { get; set; }
        //public ICommand RemoveSilenceCommand { get; set; }
        //public ICommand NormalizeCommand { get; set; }

        private PersonData _person;
        public PersonData Person
        {
            get { return _person; }
            set { _person = value; Notify(); }
        }

        private Options _options;
        public Options Options
        {
            get { return _options; }
            set { _options = value; Notify(); }
        }

        public MainViewModel(DbConnection dbConnection)
        {
            _ffmpeg = new Ffmpeg();
            _verification = new Verification(dbConnection, 200000, 200000);
            _identification = new Identification(dbConnection);
            OpenOptionsCommand = new RelayCommand(OpenOptions, canExecute => true);
            OpenVerificationCommand = new RelayCommand(OpenVerification, p => ActivateConditionVerification() == true);
            AcquirePhotoCommand = new RelayCommand(AcquirePhoto, _ffmpeg.IsBusy);
            AcquireRecordingCommand = new RelayCommand(AcquireRecording, _ffmpeg.IsBusy);
            IdentifyCommand = new RelayCommand(Identify, p => ActivateConditionIdentification() == true);
            //RemoveSilenceCommand = new RelayCommand(RemoveSilence, canExecute => true);
            //NormalizeCommand = new RelayCommand(Normalize, canExecute => true);
            Person = new PersonData();
            Options = new Options(AcquisitionMethod.FromDisk, SilenceRemoval.No, 
                VerificationMethod.FaceAndSpeech, IdentificationMethod.FaceAndSpeech);

            
        }

        private bool ActivateConditionVerification()
        {
            if (Options.VerificationMethod == VerificationMethod.FaceOnly && Person.Image != null)
            {
                return true;
            }
            else if (Options.VerificationMethod == VerificationMethod.SpeechOnly && Person.Samples != null)
            {
                return true;
            }
            else if (Options.VerificationMethod == VerificationMethod.FaceAndSpeech && Person.Image != null && Person.Samples != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ActivateConditionIdentification()
        {
            if (Options.IdentificationMethod == IdentificationMethod.FaceOnly && Person.Image != null)
            {
                return true;
            }
            else if (Options.IdentificationMethod == IdentificationMethod.SpeechOnly && Person.Samples != null)
            {
                return true;
            }
            else if (Options.IdentificationMethod == IdentificationMethod.FaceAndSpeech && Person.Image != null && Person.Samples != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AcquirePhoto(object parameter)
        {
            if (Options.AcquisitionMethod == AcquisitionMethod.FromCamera)
            {
                _ffmpeg.TakePicture("output.bmp", new EventHandler(AcquirePhotoHandler));
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "BMP files (*.bmp)|*.bmp";
                ofd.ShowDialog();
                string filename = ofd.FileName;
                if (!string.IsNullOrEmpty(filename))
                {
                    Person.LoadImage(filename, Options.AcquisitionMethod);
                }
            }
        }

        private void AcquireRecording(object parameter)
        {
            if (Options.AcquisitionMethod == AcquisitionMethod.FromCamera)
            {
                _ffmpeg.RecordAudio("output.wav", new EventHandler(AcquireRecordingHandler));
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "WAV files (*.wav)|*.wav";
                ofd.ShowDialog();
                string filename = ofd.InitialDirectory + ofd.FileName;
                if (!string.IsNullOrEmpty(filename))
                {
                    Person.LoadWavFile(filename, Options.AcquisitionMethod);
                    Person.RemoveSilence(Options.SilenceRemoval);
                }
            }
        }

        private void AcquirePhotoHandler(object parameter, EventArgs e)
        {
            _ffmpeg.EndEvent();

            Person.LoadImage(@"output.bmp", Options.AcquisitionMethod);
        }

        private void AcquireRecordingHandler(object parameter, EventArgs e)
        {
            _ffmpeg.EndEvent();
            
            Person.LoadWavFile(@"output.wav", Options.AcquisitionMethod);
            Person.RemoveSilence(Options.SilenceRemoval);
        }

        public void Identify(object parameter)
        {
            var result = _identification.Identify(Person, Options.IdentificationMethod);
            MessageBox.Show(string.Format("Face result: {0}\nSpeech result: {1}", result.Item1, result.Item2),
                "Results", MessageBoxButton.OK);
        }
        
        private void OpenOptions(object parameter)
        {
            WindowService.OpenOptions(this);
        }
        public void OpenVerification(object parameter)
        {
            WindowService.OpenVerification(this);
            var result = _verification.Verify(Person, Options.VerificationMethod);
            MessageBox.Show(string.Format("Face result: {0}\nSpeech result: {1}", result.Item1, result.Item2),
                "Results", MessageBoxButton.OK);
        }
    }
}
