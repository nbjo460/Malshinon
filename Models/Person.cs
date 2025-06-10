using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get;  set; }
        public string LastName {get;  set; }
        public string SecretCode { get;  set; }
        public string Type { get;  set; }
        public int NumReports { get;  set; }
        public int NumMentions { get;  set; }

        public Person() { }
        public Person(string first, string last,string code, string type, int reports, int mentions)
        {
            FirstName = first;
            LastName = last;
            SecretCode = code;
            Type = type;
            NumReports = reports;
            NumMentions = mentions;
        }
    }
}
