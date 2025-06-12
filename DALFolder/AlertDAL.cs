using Malshinon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.DALFolder
{
    public class AlertDAL : DAL
    {
        public static AlertDAL AlertDal { get; private set; } = null;

        public static AlertDAL DALBuilder()
        {
            if (AlertDal == null) AlertDal = new AlertDAL();
            return AlertDal;
        }
        public Alert GetReportsByWindowTime(int id, int minutes = 15)
        {
            try { 
            OpenConnection();
            string query = "SELECT `TARGET_ID` id, COUNT(`TARGET_ID`) AS Total, MAX(`timestamp`) AS END, MIN(`timestamp`) AS START " +
                "FROM `intelreports` " +
                $"WHERE `TARGET_ID` = {id} AND" +
                "`Timestamp` " +
                $"BETWEEN NOW() - INTERVAL {minutes} MINUTE AND NOW()" +
                " GROUP BY `TARGET_ID` " +
                "HAVING Total >= 3;";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
            reader = cmd.ExecuteReader();
            Alert alert = new Alert();
            while (reader.Read())
            {
                alert.TargetId = reader.GetInt32("id");
                alert.StartWindow = reader.GetDateTime("START");
                alert.EndWindow = reader.GetDateTime("END");
            }
            alert.Reason = "Rapid reports detected)";
            CloseConnection();
            if (alert.TargetId > -1)
            {
                AddAlert(alert);
                return alert;
            }
            else return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
        public void AddAlert(Alert alert)
        {
            try
            {
                OpenConnection();
                string query = $"INSERT INTO `ALERTS` (`TARGET_ID`, `START_WINDOW_TIME`, `END_WINDOW_TIME`, `CREATED_AT`, `REASON`) " +
                    $"VALUES (@target, @start, @end, @created, @reason)";
                cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("target", alert.TargetId);
                cmd.Parameters.AddWithValue("start", alert.StartWindow);
                cmd.Parameters.AddWithValue("end", alert.EndWindow);
                cmd.Parameters.AddWithValue("created", DateTime.Now);
                cmd.Parameters.AddWithValue("reason", alert.Reason);

                cmd.ExecuteNonQuery();
                CloseConnection();
                Console.WriteLine("Added Alert to DB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
