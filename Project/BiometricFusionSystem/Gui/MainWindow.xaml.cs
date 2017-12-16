using Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow(DbConnection dbConnection)
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel(dbConnection);
            this.DataContext = _mainViewModel;
        }

        private void PlayMedia(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            
        }

        private void StopMedia(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

    }
}
