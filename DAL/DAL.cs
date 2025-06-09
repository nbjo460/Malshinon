using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using MySql.Data.MySqlClient;

namespace Malshinon.DAL
{
    internal class DAL
    {
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private string connectKey = "server=localhost;user=root;password=;database=MALSHINON;";
        private MySqlConnection connection;
        public DAL()
        {
            connection = new MySqlConnection(connectKey);
        }
        private void OpenCoonection()
        {
            if(connection.State == System.Data.ConnectionState.Closed) connection.Open();
        }
        private void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed ||
                connection.State != System.Data.ConnectionState.Broken) connection.Close();
        }

        private MySqlDataReader CheckPerson(string _firstName, string _lastName)
        {
            OpenCoonection();
            string query = "SELECT * FROM PEOPLE WHERE FIRST_NAME = @_firstName AND LAST_NAME = @_lastName ;";
            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("_firstName", _firstName);
            cmd.Parameters.AddWithValue("_lastName", _lastName);
            reader = cmd.ExecuteReader();
            CloseConnection();
            return reader;
        }

        public bool IsExistPerson(string _firstName, string _lastName)
        {
            return CheckPerson(_firstName, _lastName).HasRows;
        }

        public Person GetPerson() 
        {
            Person person = new Person();
            while (reader.Read())
            {
                person.ID = reader.GetInt32("ID");
                person.FirstName = reader.GetString("First_Name");
                person.LastName = reader.GetString("Last_Name");
                person.SecretCode = reader.GetString("SecretCode");
                person.Type = reader.GetString("TYPE");
                person.NumMentions = reader.GetInt32("NUM_MENTIONS");
                person.NumReports = reader.GetInt32("NUM_REPORTS");
            }
            return person;
        }
        public void EditPerson() { }
        public void AddPerson() { }
        public void  DeletePerson() { }
    }
}
