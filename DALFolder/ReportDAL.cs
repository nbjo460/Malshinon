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
    public class ReportDAL : DAL
    {
        public static ReportDAL ReportDal { get; private set; } = null;

        public static ReportDAL DALBuilder()
        {
            if (ReportDal == null) ReportDal = new ReportDAL();
            return ReportDal;
        }
        public double GetAveOfReports(Person agent)
        {
            string query = "SELECT AVG(LENGTH(`TEXT`)) ave FROM `IntelReports` WHERE Reporter_id = @id";
            OpenConnection();
            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("id", agent.ID);
            reader = cmd.ExecuteReader();
            double ave = 0;
            while (reader.Read())
            {
                    ave = reader.GetDouble("ave");
            }
            CloseConnection();
            return ave;
        }
        public void InsertReportToDB(IntelReports report)
        {
            OpenConnection();
            string query = "INSERT INTO `IntelReports` ( Reporter_id, TARGET_ID, text) VALUES (@reporterId, @targetId, @text); ";

            cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("reporterId", report.ReporterId);
            cmd.Parameters.AddWithValue("targetId", report.TargetId);
            cmd.Parameters.AddWithValue("text", report.Text);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Report has added.");
            CloseConnection();
        }

        
    }
}
