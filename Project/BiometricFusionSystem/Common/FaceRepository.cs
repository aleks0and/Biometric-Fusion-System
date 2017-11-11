using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class FaceRepository 
    {
        private DbConnection _connection;
        public FaceRepository(DbConnection connection)
        {
            _connection = connection;
        }
        public byte[] GetFaceById(int id)
        {
            byte[] face = null;
            try
            {
                var cmdSelect = new SqlCommand("select Picture from dbo.FaceBiometric where ID=@ID", _connection.SqlConnection);
                cmdSelect.Parameters.Add("@ID", SqlDbType.Int, 4);
                cmdSelect.Parameters["@ID"].Value = id;
                _connection.SqlConnection.Open();
                face = (byte[])cmdSelect.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
            return face;
        }
    }
}
