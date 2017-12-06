using Camera;
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
        private PersonData _person;
        public PersonData Person
        {
            get { return _person; }
            set { _person = value; Notify(); }
        }
        
        public MainViewModel()
        {
            _ffmpeg = new Ffmpeg();
            OpenOptionsCommand = new RelayCommand(OpenOptions, canExecute => true);
            OpenVerificationCommand = new RelayCommand(OpenVerification, canExecute => true);
            AcquirePhotoCommand = new RelayCommand(AcquirePhoto, _ffmpeg.IsBusy);
            AcquireRecordingCommand = new RelayCommand(AcquireRecording, _ffmpeg.IsBusy);
            Person = new PersonData();
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
            Person.LoadImage("./output.bmp");
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
