using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Class responsible for interaction with biometric database(Person, FaceBiometric, VoiceBiometric tables)
    /// </summary>
    public class PersonRepository
    {
        private DbConnection _connection;
        private const int MaxNameLength = 50;
        /// <summary>
        /// Constructor for the class 
        /// </summary>
        /// <param name="connection">connection string to database</param>
        public PersonRepository(DbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// function returning a Person object given parameters
        /// </summary>
        /// <param name="firstName">first name of the person</param>
        /// <param name="lastName">last name of the person</param>
        /// <returns>person object with feature vectors (null if person does not exist)</returns>
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
        /// <summary>
        /// Function returning list of persons with their feature vectors
        /// </summary>
        /// <param name="recordedWord">word of which voice feature vector has to be included in each person object</param>
        /// <returns>list of persons (null on fail/not existing word)</returns>
        public List<Person> SelectPersons(string recordedWord)
        {
            List<Person> persons = new List<Person>();
            Person p = null;
            try
            {
                var select = new SqlCommand("select p.Id, p.FirstName, p.LastName, f.FeatureVector FaceFeatureVector, v.FeatureVector VoiceFeatureVector from Person p "
                    + "join FaceBiometric f on p.Id = f.Id join VoiceBiometric v on p.Id = v.Id where v.RecordedWord=@RecordedWord");
                select.Connection = _connection.SqlConnection;
                select.Parameters.Add("@RecordedWord", System.Data.SqlDbType.VarChar, MaxNameLength);
                select.Parameters["@RecordedWord"].Value = recordedWord;
                _connection.SqlConnection.Open();
                using (var reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        p = new Person()
                        {
                            Id = (int)reader[0],
                            FirstName = (string)reader[1],
                            LastName = (string)reader[2],
                            FaceFeatureVector = Person.FeatureVectorToList((string)reader[3], ' '),
                            VoiceFeatureVector = Person.FeatureVectorToList((string)reader[4], ' ')
                        };
                        persons.Add(p);
                    }
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
            return persons;
        }
        /// <summary>
        /// function enrolling new person to database
        /// </summary>
        /// <param name="person">Person object with feature vectors</param>
        /// <param name="recordedWord">word of voice feature vector in person object</param>
        public bool AddPerson(Person person, string recordedWord)
        {
            bool success = false;
            try
            {
                _connection.SqlConnection.Open();
                person.Id = GetPersonId(person.FirstName, person.LastName);
                if(person.Id == -1)
                {
                    AddPerson(person);
                    AddFace(person);
                    AddSpeech(person, recordedWord);
                    success = true;
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
            return success;
        }
        /// <summary>
        /// Function adding person to Person table
        /// </summary>
        /// <param name="person">person to be added</param>
        private void AddPerson(Person person)
        {
            var cmdInsert = new SqlCommand("INSERT INTO Person (FirstName,LastName) values(@FirstName,@LastName)");
            cmdInsert.Connection = _connection.SqlConnection;
            cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
            cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
            cmdInsert.Parameters["@FirstName"].Value = person.FirstName;
            cmdInsert.Parameters["@LastName"].Value = person.LastName;
            cmdInsert.ExecuteNonQuery();
            person.Id = GetPersonId(person.FirstName, person.LastName);
        }
        /// <summary>
        /// Function returning id of person if this person exists in database
        /// </summary>
        /// <param name="firstName">first name of the person</param>
        /// <param name="lastName">last name of the person</param>
        /// <returns>id of the found person (-1 on fail/person not found)</returns>
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
                while (reader.Read())
                {
                    ret = (int)reader[0];
                }
            }
            return ret;
        }
        /// <summary>
        /// Adds face feature vector of given person to FaceBiometric table
        /// </summary>
        /// <param name="person">person containing a face feature vector</param>
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
        /// <summary>
        /// Adds voice feature vector of given person to VoiceBiometric table
        /// </summary>
        /// <param name="person">person containing a voice feature vector</param>
        /// <param name="recordedWord">word represented by the voice feature vector</param>
        private void AddSpeech(Person person, string recordedWord)
        {
            var insert = new SqlCommand("INSERT INTO VoiceBiometric (Id,FeatureVector,RecordedWord) values(@Id,@FeatureVector,@RecordedWord)");
            insert.Connection = _connection.SqlConnection;
            insert.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            insert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarChar);
            insert.Parameters.Add("@RecordedWord", System.Data.SqlDbType.VarChar);
            insert.Parameters["@Id"].Value = person.Id;
            insert.Parameters["@FeatureVector"].Value = person.VoiceFeatureVectorToString(' ');
            insert.Parameters["@RecordedWord"].Value = recordedWord;
            insert.ExecuteNonQuery();
        }
        private bool CheckIfSpeechExists(Person person, string recordedWord)
        {
            var cmd = new SqlCommand("SELECT v.VoiceId FROM VoiceBiometric v WHERE v.Id=@Id AND v.RecordedWord=@RecordedWord");
            cmd.Connection = _connection.SqlConnection;
            cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            cmd.Parameters.Add("@RecordedWord", System.Data.SqlDbType.VarChar);
            cmd.Parameters["@Id"].Value = person.Id;
            cmd.Parameters["@RecordedWord"].Value = recordedWord;

            int id = -1;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = (int)reader[0];
                }
            }
            return id != -1;
        }
        public bool AddSpeechToExistingPerson(Person person, string recordedWord)
        {
            bool success = false;
            try
            {
                _connection.SqlConnection.Open();
                if (GetPersonId(person.FirstName, person.LastName) == -1
                    || CheckIfSpeechExists(person, recordedWord))
                {
                    success = false;
                }
                else
                {
                    AddSpeech(person, recordedWord);
                    success = true;
                }
            }
            catch(Exception e)
            {
                success = false;
            }
            finally
            {
                _connection.SqlConnection.Close();
            }

            return success;
        }
        public List<string> SelectAllWords()
        {
            var words = new List<string>();
            try
            {
                _connection.SqlConnection.Open();
                var select = new SqlCommand("SELECT DISTINCT v.RecordedWord FROM VoiceBiometric v");
                select.Connection = _connection.SqlConnection;
                using (var reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string word = (string)reader[0];
                        words.Add(word);
                    }
                }
            }
            catch(Exception e)
            {
                words = null;
            }
            finally
            {
                _connection.SqlConnection.Close();
            }
            return words;
        }
    }
}
