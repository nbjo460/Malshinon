using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Malshinon.DALFolder;
using Malshinon.logic;
using Malshinon.Models;

namespace Malshinon
{
    internal class Program
    {
        static void run()
        {
            DAL d = DAL.DALBuilder();

            
            Person Agent = AgentEntry();
            InsertIntel(Agent);
        }
        private static Person AgentEntry()
        {
            PersonDAL PersonDal = PersonDAL.PersonDal;
            string firstName, lastName;
            while (true)
            {
                Console.WriteLine("Enter your's first name:");
                firstName = Console.ReadLine();
                Console.WriteLine("Enter your's last name:");
                lastName = Console.ReadLine();
                if (AnalyzeName.CanBeName(firstName) && AnalyzeName.CanBeName(lastName))
                {
                    break;
                }
                Console.WriteLine("Wrong Name.\n");
            }
            if (PersonDal.IsExistPerson(firstName, lastName))
            {
                Console.WriteLine($"Hi {firstName} {lastName}");
            }
            else
            {
                PersonLogic.CreateNewPerson(firstName, lastName);
            }
            return PersonDal.GetPerson(firstName, lastName);
        }
        private static void InsertIntel(Person agent)
        {
            DAL dal = DAL.dal;
            Console.WriteLine("Enter a free text Report.");
            string report = Console.ReadLine();
            report = report.Trim();

            List<string[]> fullNames = AnalyzeName.GetFullNames(report);
            foreach (string[] fullName in fullNames)
            {
                Person target = PersonLogic.CreateNewPerson(fullName[0], fullName[1]); 
                IntelReports intelReport = new IntelReports(agent.ID, target.ID, report, DateTime.Now);
                ReportDAL.ReportDal.InsertReportToDB(intelReport);

                agent.NumReports += 1;
                target.NumMentions += 1;
                PersonDAL.PersonDal.EditPerson(agent);
                PersonDAL.PersonDal.EditPerson(target); 

                PersonLogic.UpdateTypes(agent, target);
            }
        }
        static void Main(string[] args)
        {
            run();
        }
    }
}
