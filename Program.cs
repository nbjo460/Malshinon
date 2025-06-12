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
            AlertDAL.DALBuilder();
            Menu();
                    
        }
       
        private static void Menu()
        {
            Person agent = EntryMenu();
            IntelMenu(agent);
            
        }
        static void IntelMenu(Person agent)
        {
            string report;
            while (true)
            {
                Console.WriteLine("Enter a free text Report.");
                report = Console.ReadLine();
                if (report.Length > 10) break;
                Console.WriteLine("The report is too short!");
            }

            InsertIntel(agent, report);

        }
        static Person EntryMenu()
        {
            string firstName, lastName;

            while (true)
            {
                Console.WriteLine("Enter your's first name:");
                firstName = Console.ReadLine();
                Console.WriteLine("Enter your's last name:");
                lastName = Console.ReadLine();
                if (AnalyzeName.IsValidateName(firstName) && AnalyzeName.IsValidateName(lastName))
                    break;
                Console.WriteLine("Wrong Name.\n");
            }

            return AgentEntry(firstName, lastName);
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
                IntelReports intelReport = new IntelReports(agent.ID, target.ID, report, DateTime.Now);
                ReportDAL.ReportDal.InsertReportToDB(intelReport);

                agent.NumReports += 1;
                target.NumMentions += 1;
                PersonDAL.PersonDal.EditPerson(agent);
                PersonDAL.PersonDal.EditPerson(target); 

                PersonLogic.UpdateTypes(agent, target);
                Alert alert = AlertDAL.AlertDal.GetReportsByWindowTime(target.ID, 15);
                if (alert != null)Console.WriteLine(alert);
            }
        }
        static void Main(string[] args)
        {
            run();
        }
    }
}
