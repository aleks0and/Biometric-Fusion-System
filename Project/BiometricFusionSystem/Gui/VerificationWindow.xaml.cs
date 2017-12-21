using Gui.Utility;
using Gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gui
{
    /// <summary>
    /// Interaction logic for VerificationWindow.xaml
    /// </summary>
    public partial class VerificationWindow : Window
    {
        private MainViewModel _mainViewModel;
        public VerificationWindow(BindableBase viewModel)
        {
            InitializeComponent();
            _mainViewModel = viewModel as MainViewModel;
            this.DataContext = _mainViewModel;
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
