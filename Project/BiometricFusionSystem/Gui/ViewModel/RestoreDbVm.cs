using Common;
using Gui.Model;
using Gui.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Configuration;
using Gui.Properties;

namespace Gui.ViewModel
{
    public class RestoreDbVm : BindableBase
    {
        private const string _dbName = "BiometricDB";
        private DbRestore _dbRestore;
        private string _serverName;

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; Notify(); }
        }
        public ICommand RestoreDatabaseCommand { get; set; }
        public RestoreDbVm()
        {
            RestoreDatabaseCommand = new RelayCommand(RestoreDatabase, n => true);
        }

        public void RestoreDatabase(object parameter)
        {
            _dbRestore = new DbRestore(ServerName, _dbName);
            var backupPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + _dbName + ".bak";
            _dbRestore.RestoreDatabase(backupPath);
            var dbConnection = new DbConnection(ServerName);
            SaveConnectionString(dbConnection.ConnectionString);
            WindowService.OpenMainWindow(dbConnection);
            WindowService.CloseDatabaseWindow();
        }

        private void SaveConnectionString(string connectionString)
        {
            Settings.Default.ConnectionString = connectionString;
            Settings.Default.Save();
        }
    }
}
