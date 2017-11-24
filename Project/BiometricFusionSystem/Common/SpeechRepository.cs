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
        public SpeechRepository(DbConnection connection)
        {
            _connection = connection;
        }
        /// <summary>
        /// Function loads the template from the speech database regarding
        /// </summary>
        /// <param name="id">Identification number by which the function finds the person</param>
        /// <returns> class holding the personal information - Voice recording, first name, last name and feature vector for the image</returns>
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
        /// <summary>
        /// Function responsible for saving the personal data in the speech database.
        /// </summary>
        /// <param name="person"> class holding the information about the person: voice recording, first name, last name, feature vector of the person</param>
        public void SaveSpeech(Person person)
        {
            try
            {
                var cmdInsert = new SqlCommand("INSERT INTO VoiceBiometric (Speech,FeatureVector,FirstName,LastName)" +
                    " values(@Speech,@FeatureVector,@FirstName,@LastName)");
                cmdInsert.Connection = _connection.SqlConnection;
                cmdInsert.Parameters.Add("@Speech", System.Data.SqlDbType.VarBinary);
                cmdInsert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarChar);
                cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters["@Speech"].Value = person.VoiceRecording;
                cmdInsert.Parameters["@FeatureVector"].Value = person.VoiceFeatureVector;
                cmdInsert.Parameters["@FirstName"].Value = person.FirstName;
                cmdInsert.Parameters["@LastName"].Value = person.LastName;
                _connection.SqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
        }

    }
}
