using Common;
using Gui.Model;
using Gui.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gui.ViewModel
{
    public class AddPersonViewModel : BindableBase
    {
        private PersonToAdd _person;
        private PersonRepository _personRepository;
        private ObservableCollection<string> _wordsInDatabase;
        public ICommand SelectImagesCommand { get; set; }
        public ICommand SelectSpeechRecordingsCommand { get; set; }
        public ICommand AddPersonToDatabaseCommand { get; set; }
        public PersonToAdd Person
        {
            get { return _person; }
            set { _person = value;  Notify(); }
        }
        public ObservableCollection<string> WordsInDatabase
        {
            get { return _wordsInDatabase; }
            set { _wordsInDatabase = value; Notify(); }
        }

        public AddPersonViewModel(DbConnection dbConnection)
        {
            _personRepository = new PersonRepository(dbConnection);
            Person = new PersonToAdd();
            SelectImagesCommand = new RelayCommand(SelectImages, canExecute => true);
            SelectSpeechRecordingsCommand = new RelayCommand(SelectSpeechRecordings, canExecute => true);
            AddPersonToDatabaseCommand = new RelayCommand(AddPersonToDatabase, CanAddPerson);
            LoadWordsFromDatabase();
        }

        private bool CanAddPerson(object parameter)
        {
            return !(string.IsNullOrEmpty(_person.WordToAdd) ||
                _person.ImagePathList == null || _person.ImagePathList.Count == 0 ||
                _person.SpeechPathList == null || _person.SpeechPathList.Count == 0 ||
                string.IsNullOrEmpty(_person.FirstName) ||
                string.IsNullOrEmpty(_person.LastName));
        }
        private void LoadWordsFromDatabase() 
        {
            List<string> loadedWords;
            loadedWords = _personRepository.SelectAllWords();
            if (loadedWords != null)
            {
                _wordsInDatabase = new ObservableCollection<string>(loadedWords);
            }
            else
            {
                MessageBox.Show("An error has occured during loading words from database");
            }
        }
        private void SelectImages (object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "BMP files (*.bmp)|*.bmp";
            fileDialog.Multiselect = true;
            bool? result = fileDialog.ShowDialog();
            if(result == true)
            {
                if (fileDialog.FileNames.Length != 0)
                {
                    ObservableCollection<string> imagePaths = new ObservableCollection<string>(fileDialog.FileNames.ToList());
                    Person.ImagePathList = imagePaths;
                }
                else
                {
                    MessageBox.Show("Select at least one picture");
                }
            }
        }
        private void SelectSpeechRecordings(object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "WAV files (*.wav)|*.wav";
            fileDialog.Multiselect = true;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                if (fileDialog.FileNames.Length != 0)
                {
                    ObservableCollection<string> speechPaths = new ObservableCollection<string>(fileDialog.FileNames.ToList());
                    Person.SpeechPathList = speechPaths;
                }
                else
                {
                    MessageBox.Show("Select at least one recording");
                }
            }
        }
        private void AddPersonToDatabase(object parameter)
        {
            InputDataProcessing dataProcessing = new InputDataProcessing(_person);
            AddFaceAndVoiceToDatabase(dataProcessing, _person.WordToAdd);
        }
        private void AddFaceAndVoiceToDatabase (InputDataProcessing dataProcessing, string wordToAdd)
        {
            Person p = new Person();
            p.FaceFeatureVector = dataProcessing.ExtractFaceFeatureVector();
            p.VoiceFeatureVector = dataProcessing.ExtractSpeechFeatureVector();
            p.FirstName = _person.FirstName;
            p.LastName = _person.LastName;
            bool result = _personRepository.AddPerson(p, wordToAdd);
            if(result)
            {
                WordsInDatabase.Add(_person.WordToAdd);
                MessageBox.Show(p.FirstName + " " + p.LastName + " successfully added to database.");
            }
            else
            {
                MessageBox.Show("Unable to add " + p.FirstName + " " + p.LastName + " to database.");
            }
        }
    }
}
