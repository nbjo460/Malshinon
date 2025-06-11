using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            
            PersonDAL.DALBuilder();
            ReportDAL.DALBuilder();

            Menu();
                    
        }
       
        private static void Menu()
        {
            string firstName, lastName, report;

            while (true)
            {
                Console.WriteLine("Enter your's first name:");
                firstName = Console.ReadLine();
                Console.WriteLine("Enter your's last name:");
                lastName = Console.ReadLine();
                if (AnalyzeName.CanBeName(firstName) && AnalyzeName.CanBeName(lastName))
                    break;
                Console.WriteLine("Wrong Name.\n");
            }
            
            Person Agent = AgentEntry(firstName, lastName);

            while (true)
            {
                Console.WriteLine("Enter a free text Report.");
                report = Console.ReadLine();
                if (report.Length > 15) break;
                Console.WriteLine("The report is too short!");
            }
            
            InsertIntel(Agent, report);

        }
        private static Person AgentEntry(string firstName, string lastName)
        {
            PersonDAL PersonDal = PersonDAL.PersonDal;
            
            if (PersonDal.IsExistPerson(firstName, lastName))
            {
                Console.WriteLine($"Hi {firstName} {lastName}");
            }
            else
            {
                return PersonLogic.CreateNewPerson(firstName, lastName);
            }
            return PersonDal.GetPerson(firstName, lastName);
        }
        private static void InsertIntel(Person agent, string report)
        {
            DAL dal = DAL.dal;
            report = report.Trim();

            List<string[]> fullNames = AnalyzeName.GetFullNames(report);
            foreach (string[] fullName in fullNames)
            {
                Person target = PersonLogic.CreateNewPerson(fullName[0].Trim(), fullName[1].Trim());
                Console.WriteLine($"agent PersonDAL.PersonDal.GetId(agent)  *********{agent.ID}\n" +
                                  $"target PersonDAL.PersonDal.GetId(target)********{target.ID}");
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
