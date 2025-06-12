using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace Malshinon.DALFolder
{
    public class DAL
    {
        public static DAL dal { get; private set; } = null;

        protected MySqlCommand cmd;
        protected MySqlDataReader reader;
        protected string connectKey = "server=localhost;user=root;password=;database=MALSHINON;";
        protected MySqlConnection connection;
        protected DAL()
        {
            connection = new MySqlConnection(connectKey);
        }

        public static DAL DALBuilder()
        {
            if (dal == null) dal = new DAL();
            return dal;
        }

        protected void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed) connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        protected void CloseConnection()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Closed ||
                    connection.State != System.Data.ConnectionState.Broken) connection.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
