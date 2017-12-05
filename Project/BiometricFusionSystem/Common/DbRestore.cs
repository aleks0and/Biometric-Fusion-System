using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DbRestore
    {
        private string _serverName;
        private string _dbName;

        public DbRestore(string serverName, string dbName)
        {
            _serverName = serverName;
            _dbName = dbName;
        }

        public void RestoreDatabase(string backupPath)
        {
            Server server = new Server(_serverName);
            Database db = new Database(server, _dbName);
            try
            {
                db.Create();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Database already exists");
            }
            db.Refresh();
            var restore = new Restore();
            restore.NoRecovery = false;
            restore.Action = RestoreActionType.Database;
            BackupDeviceItem item = new BackupDeviceItem(backupPath, DeviceType.File);
            restore.Devices.Add(item);
            restore.Database = _dbName;
            restore.ReplaceDatabase = true;
            restore.SqlRestore(server);
            db.Refresh();
            db.SetOnline();
            server.Refresh();
        }
    }
}
