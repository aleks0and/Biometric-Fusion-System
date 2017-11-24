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
    /// <summary>
    /// Class which manages getting the information from the database
    /// </summary>
    class FaceRepository 
    {
        private DbConnection _connection;
        private const int MaxNameLength = 50;
        public FaceRepository(DbConnection connection)
        {
            _connection = connection;
        }
        /// <summary>
        /// Function loads the template from the face database regarding
        /// </summary>
        /// <param name="id">Identification number by which the function finds the person</param>
        /// <returns> class holding the personal information - image, first name, last name and feature vector for the image</returns>
        public Person GetFaceById(int id)
        {
            Person person = null;
            try
            {
                var cmdSelect = new SqlCommand("select Picture, FirstName, LastName, FeatureVector from dbo.FaceBiometric where ID=@ID", _connection.SqlConnection);
                cmdSelect.Parameters.Add("@ID", SqlDbType.Int, 4);
                cmdSelect.Parameters["@ID"].Value = id;
                _connection.SqlConnection.Open();
                var reader = cmdSelect.ExecuteReader();
                person = new Person()
                {
                    Id = id,
                    FaceImage = (byte[])reader[0],
                    FirstName = (string)reader[1],
                    LastName = (string)reader[2],
                    FaceFeatureVector = ((double[])reader[3]).ToList()
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
        /// Function responsible for saving the person data in the face database.
        /// </summary>
        /// <param name="person"> class holding the information about the person: picture, first name, last name, feature vector of the person</param>
        public void SaveFace(Person person)
        {
            try
            {
                var cmdInsert = new SqlCommand("INSERT INTO FaceBiometric (Picture,FeatureVector,FirstName,LastName) values(@Picture,@FeatureVector,@FirstName,@LastName)");
                cmdInsert.Connection = _connection.SqlConnection;
                cmdInsert.Parameters.Add("@Picture", System.Data.SqlDbType.Image);
                cmdInsert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarChar);
                cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters["@Picture"].Value = person.FaceImage;
                cmdInsert.Parameters["@FeatureVector"].Value = person.FaceFeatureVector;
                cmdInsert.Parameters["@FirstName"].Value = person.FirstName;
                cmdInsert.Parameters["@LastName"].Value = person.LastName;
                _connection.SqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
            catch(SqlException ex)
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
