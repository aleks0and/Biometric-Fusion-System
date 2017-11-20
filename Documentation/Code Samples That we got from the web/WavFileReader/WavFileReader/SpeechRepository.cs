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
        private const int MaxNameLength = 50;
        SpeechRepository(DbConnection connection)
        {
            _connection = connection;
        }
        public Person GetSpeechById(int id)
        {
            Person person = null;
            try
            {
                var cmdSelect = new SqlCommand("select Speech, FirstName, LastName, FeatureVector from dbo.FaceBiometric where ID=@ID", _connection.SqlConnection);
                cmdSelect.Parameters.Add("@ID", SqlDbType.Int, 4);
                cmdSelect.Parameters["@ID"].Value = id;
                _connection.SqlConnection.Open();
                var reader = cmdSelect.ExecuteReader();
                person = new Person()
                {
                    Id = id,
                    VoiceRecording = (byte[])reader[0],
                    FirstName = (string)reader[1],
                    LastName = (string)reader[2],
                    VoiceFeatureVector = ((double[])reader[3]).ToList()
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
            return person;
        }
        public void SaveSpeech(byte[] speech, List<double> featureVector, string firstName, string lastName)
        {
            try
            {
                var cmdInsert = new SqlCommand("INSERT INTO FaceBiometric (Speech,FeatureVector,FirstName,LastName)" +
                    " values(@Speech,@FeatureVector,@FirstName,@LastName");
                cmdInsert.Parameters.Add("@Speech", System.Data.SqlDbType.Image);
                cmdInsert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarBinary);
                cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters["@Speech"].Value = speech;
                cmdInsert.Parameters["@FeatureVector"].Value = featureVector.ToArray();
                cmdInsert.Parameters["@FirstName"].Value = firstName;
                cmdInsert.Parameters["@LastName"].Value = lastName;
                _connection.SqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                _connection.SqlConnection.Close();
            }
        }

    }
}
