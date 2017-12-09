using Common;
using Gui.Model;
using Gui.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if(!string.IsNullOrEmpty(Settings.Default.ConnectionString))
            {
                var dbConnection = new DbConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString, true);
                WindowService.OpenMainWindow(dbConnection);
            }
            else
            {
                WindowService.OpenDatabaseRestore();
            }
        }
    }
}
