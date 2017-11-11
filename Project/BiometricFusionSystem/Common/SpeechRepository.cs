using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Common
{
    class SpeechRepository
    {
        private DbConnection _connection;
        SpeechRepository(DbConnection connection)
        {
            _connection = connection;
        }
        public byte[] GetSpeechById(int id)
        {
            byte[] speech = null;
            try
            {
                var cmdSelect = new SqlCommand("select Speech from dbo.VoiceBiometric where ID=@ID", _connection.SqlConnection);
                cmdSelect.Parameters.Add("@ID", SqlDbType.Int, 4);
                cmdSelect.Parameters["@ID"].Value = id;
                _connection.SqlConnection.Open();
                speech = (byte[])cmdSelect.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
            return speech;
        }

    }
}
