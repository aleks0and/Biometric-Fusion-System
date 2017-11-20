using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DbConnection
    {
        private SqlConnection _sqlConnection;
        public SqlConnection SqlConnection { get { return _sqlConnection; } }
        public DbConnection ()
        {
            //"Server=localhost\\MSSQLSERVER; Data Source=ServerName; Initial Catalog = BiometricDB; User ID = userId; Password = password";
            _sqlConnection.ConnectionString = "Server=localhost\\SQLEXPRESS; Data Source=DESKTOP-BDCV57Q\\SQLEXPRESS; Initial Catalog = BiometricDB; User ID = userId; Password = password";
            _sqlConnection.FireInfoMessageEventOnUserErrors = false;
        }

    }
}
