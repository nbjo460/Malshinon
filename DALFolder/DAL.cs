using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using MySql.Data.MySqlClient;

namespace Malshinon.DALFolder
{
     public class DAL
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
            string query = "SELECT * FROM PEOPLE WHERE FIRST_NAME = @firstName AND LAST_NAME = @lastName;";
            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("firstName", _firstName);
            cmd.Parameters.AddWithValue("lastName", _lastName);
            reader = cmd.ExecuteReader();
            return reader;
        }


        public bool IsExistPerson(string _firstName, string _lastName)
        {
            bool Exist = CheckPerson(_firstName, _lastName).HasRows;
            CloseConnection();
            return Exist;
        }
        public Person GetPerson(string _firstName, string _lastName) 
        {
            Person person = new Person();
            reader = CheckPerson(_firstName, _lastName);
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

            CloseConnection();

            return person;
        }
        public void EditPerson() { }
        public void AddPerson(Person person) 
        {
            OpenCoonection();
            string query = 
            "INSERT INTO PEOPLE (FIRST_NAME, LAST_NAME, SECRET_CODE, TYPE, NUM_REPORTS, NUM_MENTIONS)" +
            "VALUES (@firstname, @lastname, @secretcode, @type, @num_reports, @num_mentions);";
            
            cmd = new MySqlCommand(query, connection);
            
            cmd.Parameters.AddWithValue("firstName", person.FirstName);
            cmd.Parameters.AddWithValue("lastName", person.LastName);
            cmd.Parameters.AddWithValue("secretcode", person.SecretCode);
            cmd.Parameters.AddWithValue("type", person.Type);
            cmd.Parameters.AddWithValue("num_reports", person.NumReports);
            cmd.Parameters.AddWithValue("num_mentions", person.NumMentions);
            
            reader = cmd.ExecuteReader();
            CloseConnection();
        }
        public void  DeletePerson() { }
    }
}
