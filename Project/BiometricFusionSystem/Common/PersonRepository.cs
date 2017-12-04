using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PersonRepository
    {
        private DbConnection _connection;
        private const int MaxNameLength = 50;
        public PersonRepository(DbConnection connection)
        {
            _connection = connection;
        }
        public Person GetPerson(string firstName, string lastName)
        {
            Person p = null;
            try
            { 
                var select = new SqlCommand("select p.Id, p.FirstName, p.LastName, f.FeatureVector FaceFeatureVector, v.FeatureVector VoiceFeatureVector from Person p " 
                    + "join FaceBiometric f on p.Id = f.Id join VoiceBiometric v on p.Id = v.Id where p.FirstName=@FirstName and p.LastName=@LastName");
                select.Connection = _connection.SqlConnection;
                select.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
                select.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
                select.Parameters["@FirstName"].Value = firstName;
                select.Parameters["@LastName"].Value = lastName;
                _connection.SqlConnection.Open();
                using (var reader = select.ExecuteReader())
                {
                    reader.Read();
                    p = new Person()
                    {
                        Id = (int)reader[0],
                        FirstName = (string)reader[1],
                        LastName = (string)reader[2],
                        FaceFeatureVector = Person.FeatureVectorToList((string)reader[3], ' '),
                        VoiceFeatureVector = Person.FeatureVectorToList((string)reader[4], ' ')
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
            return p;
        }
        public void AddPerson(Person person)
        {
            try
            {
                var cmdInsert = new SqlCommand("INSERT INTO Person (FirstName,LastName) values(@FirstName,@LastName)");
                cmdInsert.Connection = _connection.SqlConnection;
                cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters["@FirstName"].Value = person.FirstName;
                cmdInsert.Parameters["@LastName"].Value = person.LastName;
                _connection.SqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
                person.Id = GetPersonId(person.FirstName, person.LastName);
                AddFace(person);
                AddSpeech(person);
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

        private int GetPersonId(string firstName, string lastName)
        {
            int ret = -1;

            var select = new SqlCommand("select Id from Person where FirstName=@FirstName and LastName=@LastName");
            select.Connection = _connection.SqlConnection;
            select.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, firstName.Length);
            select.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, lastName.Length);
            select.Parameters["@FirstName"].Value = firstName;
            select.Parameters["@LastName"].Value = lastName;
            using (var reader = select.ExecuteReader())
            {
                reader.Read();
                ret = (int)reader[0];
            }
            return ret;
        }

        private void AddFace(Person person)
        {
            var insert = new SqlCommand("INSERT INTO FaceBiometric (Id,FeatureVector) values(@Id,@FeatureVector)");
            insert.Connection = _connection.SqlConnection;
            insert.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            insert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarChar);
            insert.Parameters["@Id"].Value = person.Id;
            insert.Parameters["@FeatureVector"].Value = person.FaceFeatureVectorToString(' ');

            insert.ExecuteNonQuery();
        }
        private void AddSpeech(Person person)
        {
            var insert = new SqlCommand("INSERT INTO VoiceBiometric (Id,FeatureVector) values(@Id,@FeatureVector)");
            insert.Connection = _connection.SqlConnection;
            insert.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            insert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarChar);
            insert.Parameters["@Id"].Value = person.Id;
            insert.Parameters["@FeatureVector"].Value = person.VoiceFeatureVectorToString(' ');

            insert.ExecuteNonQuery();
        }
    }
}
