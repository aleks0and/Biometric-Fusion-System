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
using System.Windows.Shapes;

namespace Gui
{
    /// <summary>
    /// Interaction logic for AddPerson.xaml
    /// </summary>
    public partial class AddPerson : Window
    {
        public AddPerson(DbConnection dbConnection)
        {
            InitializeComponent();
            this.DataContext = new AddPersonViewModel(dbConnection);
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = e.Source as TextBox;
            MultiBindingExpression binding = BindingOperations.GetMultiBindingExpression(textbox, TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
