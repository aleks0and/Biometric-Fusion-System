using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    public class PersonToAdd : BindableBase
    {
        private ObservableCollection<string> _imagePathList;
        private ObservableCollection<string> _speechPathList;
        private string _firstName;
        private string _lastName;
        private string _wordToAdd;

        public ObservableCollection<string> ImagePathList
        {
            get { return _imagePathList; }
            set { _imagePathList = value; Notify(); }
        }
        public ObservableCollection<string> SpeechPathList
        {
            get { return _speechPathList; }
            set { _speechPathList = value; Notify(); }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value;  Notify(); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; Notify(); }
        }
        public string WordToAdd
        {
            get { return _wordToAdd; }
            set { _wordToAdd = value; Notify(); }
        }

    }
}
