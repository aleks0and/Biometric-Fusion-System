using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    public class WindowService
    {

        public void OpenOptions(BindableBase viewmodel)
        {
            var optionsWindow = new OptionsWindow(viewmodel);
            optionsWindow.ShowDialog();
        }

        public void OpenVerifiaction(BindableBase viewmodel)
        {
            var window = new VerificationWindow(viewmodel);
            window.ShowDialog();
        }
    }
}
