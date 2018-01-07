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
        public ICommand SelectImagesCommand;
        public ICommand SelectSpeechRecordingsCommand;
        public ICommand AddPersonToDatabaseCommand;
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
            SelectImagesCommand = new RelayCommand(SelectImages, canExecute => true);
            SelectSpeechRecordingsCommand = new RelayCommand(SelectSpeechRecordings, canExecutre => true);
            AddPersonToDatabaseCommand = new RelayCommand(AddPersonToDatabase, canExecute => true);
            LoadWordsFromDatabase();
        }
        private void LoadWordsFromDatabase() 
        {
            List<string> loadedWords;
            loadedWords = _personRepository.SelectAllWords();
            if (loadedWords != null)
                _wordsInDatabase = new ObservableCollection<string>(loadedWords);
            else
                MessageBox.Show("An error has occured during loading words from database");
        }
        private void SelectImages (object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "BMP files (*.bmp)|*.bmp";
            fileDialog.Multiselect = true;
            if (fileDialog.FileNames.Length != 0)
            {
                ObservableCollection<string> imagePaths = new ObservableCollection<string>(fileDialog.FileNames.ToList());
                _person.ImagePathList = imagePaths;
            }
            else
                MessageBox.Show("Select at least one picture");
        }
        private void SelectSpeechRecordings(object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "WAV files (*.wav)|*.wav";
            fileDialog.Multiselect = true;
            if (fileDialog.FileNames.Length != 0)
            {
                ObservableCollection<string> speechPaths = new ObservableCollection<string>(fileDialog.FileNames.ToList());
                _person.SpeechPathList = speechPaths;
            }
            else
                MessageBox.Show("Select at least one recording");
        }
        private void AddPersonToDatabase(object parameter)
        {
            bool isAnyDataLoaded = false;
            if (_person.FirstName != null && _person.LastName != null)
            {
                InputDataProcessing dataProcessing = new InputDataProcessing(_person);

                if( _person.ImagePathList.Count != 0)
                {
                    isAnyDataLoaded = true;
                    addFaceAndVoiceToDatabase(dataProcessing, _person.WordToAdd);
                }
                else if (_person.SpeechPathList.Count != 0)
                {
                    isAnyDataLoaded = true;
                    addOneWordToDatabase(dataProcessing, _person.WordToAdd);
                }
                if (!isAnyDataLoaded)
                {
                    MessageBox.Show("Please select the data");
                }
            }
            else
                MessageBox.Show("Please Enter first and last name of the person");
        }
        private void addFaceAndVoiceToDatabase (InputDataProcessing dataProcessing, string wordToAdd)
        {
            Person p = new Person();
            p.FaceFeatureVector = dataProcessing.extractFaceFeatureVector();
            p.VoiceFeatureVector = dataProcessing.extractSpeechFeatureVector();
            p.FirstName = _person.FirstName;
            p.LastName = _person.LastName;
            _personRepository.AddPerson(p, wordToAdd);
        }
        private void addOneWordToDatabase (InputDataProcessing dataProcessing, string wordToAdd)
        {
            Person p = new Person();
            p.VoiceFeatureVector = dataProcessing.extractSpeechFeatureVector();
            p.FirstName = _person.FirstName;
            p.LastName = _person.LastName;
            _personRepository.AddSpeechToExistingPerson(p, wordToAdd);
        }
    }
}
