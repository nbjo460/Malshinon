using Malshinon.DALFolder;
using Malshinon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.logic
{
    internal static class PersonLogic
    {

        public static Person CreateNewPerson(string firstName, string lastName, string type = "reporter")
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            PersonDAL PersonDal = PersonDAL.PersonDal;
            Person p = new Person(firstName, lastName, GenerateCode(firstName + lastName), type, 0, 0);
            if (!PersonDal.IsExistPerson(firstName, lastName))
            {
                PersonDal.AddPerson(p);
                p.ID = PersonDal.GetId(p);
                Console.WriteLine($"Added new Person.\n{p.FirstName} {p.LastName} ID: {p.ID}");
            }
            else
            {
                return PersonDal.GetPerson(firstName, lastName);
            }
                return p;
        }
        public static void UpdateTypes(Person agent, Person target)
        {
            if (target.NumMentions >= 20)
            {
                Console.WriteLine($"{target.FirstName} {target.LastName} is potaential threat alert!");
            }
            ChangeStatus(agent ,true);
            ChangeStatus(target, false);
        }
        public static void ChangeStatus(Person person, bool isReporter)
        {
            string tmpType = person.Type;
            bool reporter = false;
            bool terrorist = false;


            if (person.NumReports > 0)
                reporter = true;

            if (person.NumMentions > 0)
                terrorist = true;

            if (reporter && terrorist)
                person.Type = "both";
            else if (isReporter && ReportDAL.ReportDal.GetAveOfReports(person) >= 100 && person.NumReports >= 20)
                person.Type = "potential_agent";
            else if (terrorist && !reporter)
                person.Type = "target";

            if (tmpType != person.Type)
            {
                PersonDAL.PersonDal.EditPerson(person);
                Console.WriteLine($"Status of {person.FirstName} {person.LastName} Changed to: {person.Type}");
            }
        }
        public static string GenerateCode(string name)
        {
            string upside = "";
            for (int i = name.Length - 1; i >= 0; i--)
            {
                upside += name[i];
            }
            return upside;
        }
       
    }
}
