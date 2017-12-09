using Common;
using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gui.Model
{
    public static class WindowService
    {
        private static Window _dbWindow;
        public static void OpenOptions(BindableBase viewmodel)
        {
            var optionsWindow = new OptionsWindow(viewmodel);
            optionsWindow.ShowDialog();
        }

        public static void OpenVerification(BindableBase viewmodel)
        {
            var window = new VerificationWindow(viewmodel);
            window.ShowDialog();
        }

        public static void OpenDatabaseRestore()
        {
            _dbWindow = new RestoreDbWindow();
            _dbWindow.ShowDialog();
        }

        public static void OpenMainWindow(DbConnection dbConnection)
        {
            var mainWindow = new MainWindow(dbConnection);
            mainWindow.Show();
        }

        public static void CloseDatabaseWindow()
        {
            _dbWindow.Close();
            _dbWindow = null;
        }
    }
}
