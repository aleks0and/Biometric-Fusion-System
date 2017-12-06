using Gui.Model;
using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gui.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private WindowService _service = new WindowService();
        public ICommand OpenOptionsCommand { get; set; }
        public ICommand OpenVerificationCommand { get; set; }
        
        public MainViewModel()
        {
            OpenOptionsCommand = new RelayCommand(OpenOptions, canExecute => true);
            OpenVerificationCommand = new RelayCommand(OpenVerification, canExecute => true);
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
