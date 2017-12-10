using Camera;
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
        private Identification _identification;
        private Ffmpeg _ffmpeg;
        public ICommand OpenOptionsCommand { get; set; }
        public ICommand OpenVerificationCommand { get; set; }
        public ICommand AcquirePhotoCommand { get; set; }
        public ICommand AcquireRecordingCommand { get; set; }
        public ICommand IdentifyCommand { get; set; }
        private PersonData _person;
        public PersonData Person
        {
            get { return _person; }
            set { _person = value; Notify(); }
        }
        
        public MainViewModel(DbConnection dbConnection)
        {
            _ffmpeg = new Ffmpeg();
            _identification = new Identification(dbConnection);
            OpenOptionsCommand = new RelayCommand(OpenOptions, canExecute => true);
            OpenVerificationCommand = new RelayCommand(OpenVerification, canExecute => true);
            AcquirePhotoCommand = new RelayCommand(AcquirePhoto, _ffmpeg.IsBusy);
            AcquireRecordingCommand = new RelayCommand(AcquireRecording, _ffmpeg.IsBusy);
            IdentifyCommand = new RelayCommand(Identify, canExecute => true); //p => Person.Image != null && Person.Samples != null
            Person = new PersonData();
        }

        private void AcquirePhoto(object parameter)
        {
            //_ffmpeg.TakePicture("output.bmp", new EventHandler(AcquirePhotoHandler));
            OpenFileDialog ofd = new OpenFileDialog();
            string strfilename;
            ofd.ShowDialog();
            strfilename = ofd.FileName;
            Person.LoadImage(strfilename);
        }

        private void AcquireRecording(object parameter)
        {
            //_ffmpeg.RecordAudio("output.wav", new EventHandler(AcquireRecordingHandler));
            OpenFileDialog ofd = new OpenFileDialog();
            string strfilename;
            ofd.ShowDialog();
            strfilename = ofd.InitialDirectory + ofd.FileName;
            Person.LoadWavFile(strfilename);
        }

        private void AcquirePhotoHandler(object parameter, EventArgs e)
        { 
            _ffmpeg.EndEvent();
            MessageBox.Show("Photo acquired");
            Person.LoadImage(@"output.bmp");
        }
        private void AcquireRecordingHandler(object parameter, EventArgs e)
        {
            _ffmpeg.EndEvent();
            MessageBox.Show("Recording acquired");
            Person.LoadWavFile(@"output.wav");
        }

        private void Identify(object parameter)
        {
            _identification.Identify(Person);
        }

        private void OpenOptions(object parameter)
        {
            WindowService.OpenOptions(this);
        }
        private void OpenVerification(object parameter)
        {
            WindowService.OpenVerification(this);
        }
    }
}
