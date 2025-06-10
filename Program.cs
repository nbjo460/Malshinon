using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Malshinon.DALFolder;
using Malshinon.Models;

namespace Malshinon
{
    internal class Program
    {
        static void run()
        {
            DAL dal = new DAL();
            Person Agent = Name(dal);
            Intel(dal, Agent);
        }
        private static Person Name(DAL dal)
        {
            string firstName, lastName;
            while (true)
            {
                Console.WriteLine("Enter your's first name:");
                firstName = Console.ReadLine();
                Console.WriteLine("Enter your's last name:");
                lastName = Console.ReadLine();
                if (AnalyzeReport.CanBeName(firstName) && AnalyzeReport.CanBeName(lastName))
                {
                    break;
                }
                Console.WriteLine("Wrong Name.\n");
            }
            if (dal.IsExistPerson(firstName, lastName))
            {
                Console.WriteLine($"Hi {firstName} {lastName}");
            }
            else
            {
                CreateName(dal, firstName, lastName);
            }
            return dal.GetPerson(firstName, lastName);
        }
        private static void CreateName(DAL dal, string firstName, string lastName, string type = "reporter")
        {
            Person p = new Person(firstName, lastName, GenerateCode(firstName + lastName), type, 0, 0);
            if (!dal.IsExistPerson(firstName, lastName))
            {
                dal.AddPerson(p);
                Console.WriteLine($"Added new Person.\n{p.FirstName} {p.LastName}");
            }
            else
            {
                Console.WriteLine($"The person: {p.FirstName} {p.LastName}. is Already Exist!");
            }
        }
        private static void Intel(DAL dal, Person agent)
        {
            Console.WriteLine("Enter a free text Report.");
            string report = Console.ReadLine();
            report = report.Trim();

            List<string[]> fullNames = AnalyzeReport.GetFullNames(report);
            foreach (string[] fullName in fullNames)
            {
                CreateName(dal, fullName[0], fullName[1]);
                Person target = dal.GetPerson(fullName[0], fullName[1]);
                IntelReports intelReport = new IntelReports(agent.ID, target.ID, report, DateTime.Now);
                dal.InsertReportToDB(intelReport);
                dal.IncrementReporter(agent);
                dal.IncrementTarget(target);
                UpdateTypes(agent, target);
            }
        }
        private static void UpdateTypes(Person agent, Person target)
        {
            if (target.NumMentions  >= 20)
            {
                Console.WriteLine($"{target.FirstName} {target.LastName} is potaential threat alert!");
            }
            ChangeStatus(agent);
            ChangeStatus(target);
        }
        private static void ChangeStatus(Person person)
        {
            string tmpType = person.Type;
            bool reporter = false;
            bool terrorist = false;


            if (person.NumReports > 0)
                reporter = true;

            if (person.NumMentions > 1)
                terrorist = true;

            if (reporter && terrorist)
                person.Type = "both";
            else if (DAL.dal.GetAveOfReports(person) >= 100 && person.NumReports >= 20)
                person.Type = "potential_agent";
            else if (terrorist && !reporter)
                person.Type = "target";

            if (tmpType != person.Type)
            {
                DAL.dal.EditPerson(person);
                Console.WriteLine($"Status of {person.FirstName} {person.LastName} Changed to: {person.Type}");
            }
        }
        private static string GenerateCode(string name)
        {
            string upside = "";
            for(int i = name.Length-1; i >= 0; i--)
            {
                upside += name[i];
            }
            return upside;
        }
        static void Main(string[] args)
        {
            run();
        }
    }
}
