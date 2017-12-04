using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            _sqlConnection.ConnectionString = @"Data Source=DESKTOP-BDCV57Q\SQLEXPRESS;Initial Catalog=BiometricDB;Integrated Security=True";
            _sqlConnection.FireInfoMessageEventOnUserErrors = false;
        }

    }
}
