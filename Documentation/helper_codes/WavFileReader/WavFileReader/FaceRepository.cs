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
        private const int MaxNameLength = 50;
        public FaceRepository(DbConnection connection)
        {
            _connection = connection;
        }
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

        public void SaveFace(byte[] img, string featureVector, string firstName, string lastName)
        {
            try
            {
                var cmdInsert = new SqlCommand("INSERT INTO FaceBiometric (Picture,FeatureVector,FirstName,LastName) values(@Picture,@FeatureVector,@FirstName,@LastName)");
                cmdInsert.Connection = _connection.SqlConnection;
                cmdInsert.Parameters.Add("@Picture", System.Data.SqlDbType.Image);
                cmdInsert.Parameters.Add("@FeatureVector", System.Data.SqlDbType.VarChar);
                cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, MaxNameLength);
                cmdInsert.Parameters["@Picture"].Value = img;
                cmdInsert.Parameters["@FeatureVector"].Value = featureVector;
                cmdInsert.Parameters["@FirstName"].Value = firstName;
                cmdInsert.Parameters["@LastName"].Value = lastName;
                _connection.SqlConnection.Open();
                cmdInsert.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {

            }
            finally
            {
                _connection.SqlConnection.Close();
            }
        }
    }
}
