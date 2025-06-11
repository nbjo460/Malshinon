using Malshinon.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.DALFolder
{

    public class PersonDAL : DAL
    {
        public static PersonDAL PersonDal { get; private set; } = null;

        public static PersonDAL DALBuilder()
        {
            if (PersonDal == null) PersonDal = new PersonDAL();
            return PersonDal;
        }

        private MySqlDataReader CheckPerson(string _firstName, string _lastName)
        {
            OpenConnection();
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
                person.SecretCode = reader.GetString("Secret_Code");
                person.Type = reader.GetString("TYPE");
                person.NumMentions = reader.GetInt32("NUM_MENTIONS");
                person.NumReports = reader.GetInt32("NUM_REPORTS");
            }

            CloseConnection();

            return person;
        }
        public void EditPerson(Person newPerson)
        {
            OpenConnection();
            string query = "UPDATE `PEOPLE` SET " +
                "LAST_NAME = @last, FIRST_NAME = @first, SECRET_CODE = @code, TYPE = @type, NUM_REPORTS = @reports, NUM_MENTIONS = @mentions" +
                " WHERE `ID` = @id";
            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("last", newPerson.LastName);
            cmd.Parameters.AddWithValue("first", newPerson.FirstName);
            cmd.Parameters.AddWithValue("code", newPerson.SecretCode);
            cmd.Parameters.AddWithValue("type", newPerson.Type);
            cmd.Parameters.AddWithValue("reports", newPerson.NumReports);
            cmd.Parameters.AddWithValue("mentions", newPerson.NumMentions);
            cmd.Parameters.AddWithValue("id", newPerson.ID);

            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public void AddPerson(Person person)
        {
            OpenConnection();
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
        public void DeletePerson() { }
        public int GetId(Person p)
        {
            OpenConnection();
            string query = "SELECT ID id FROM `People` WHERE FIRST_NAME = @first AND LAST_NAME = @last;";
            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("last", p.LastName.Trim());
            cmd.Parameters.AddWithValue("first", p.FirstName.Trim());
            reader = cmd.ExecuteReader();
            int id = 0;
            while (reader.Read())
            {
                id = reader.GetInt32("id");
            }
            CloseConnection();
            return id;
        }
    }
}
