using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Class holding the necessary information for connection to the database
    /// </summary>
    public class DbConnection
    {
        private SqlConnection _sqlConnection;
        public SqlConnection SqlConnection { get { return _sqlConnection; } }
        public string ConnectionString { get; set; }
        /// <summary>
        /// Function which sets the necessary credentials which are needed for the connection
        /// Server - localhost\\ followed by the name of the server application
        /// Data Source - name of the server
        /// Catalog - name of the database
        /// User ID
        /// Password
        /// example use: Server=localhost\\MSSQLSERVER; Data Source=ServerName; Initial Catalog = BiometricDB; User ID = user; Password = password
        /// </summary>
        public DbConnection ()
        {
            _sqlConnection = new SqlConnection();
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var databaseName = "biometricDB.mdf";
            path = "\"" + path + "\\" + databaseName + "\"";
            ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = " + path + "; Integrated Security = True;";
            _sqlConnection.ConnectionString = ConnectionString;
            _sqlConnection.FireInfoMessageEventOnUserErrors = false;
        }

        public DbConnection(bool isTest)
        {
            if (isTest)
            {
                _sqlConnection = new SqlConnection();
                ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = \"C:\\Users\\aleks\\Desktop\\Engineering Thesis\\Project\\BiometricFusionSystem\\Common\\biometricDB.mdf\"; Integrated Security = True;";
                _sqlConnection.ConnectionString = ConnectionString;
                _sqlConnection.FireInfoMessageEventOnUserErrors = false;
            }
        }

        public DbConnection(string dataSource)
        {
            _sqlConnection = new SqlConnection();
            ConnectionString = @"Data Source=" + dataSource + @";Initial Catalog=BiometricDB;Integrated Security=True";
            _sqlConnection.ConnectionString = ConnectionString;
            _sqlConnection.FireInfoMessageEventOnUserErrors = false;
        }

        public DbConnection(string connectionString, bool fireInfoMessage)
        {
            _sqlConnection = new SqlConnection();
            ConnectionString = connectionString;
            _sqlConnection.ConnectionString = ConnectionString;
            _sqlConnection.FireInfoMessageEventOnUserErrors = fireInfoMessage;
        }
    }
}
