using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Models
{
    internal class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName {get; set;}
        public string SecretCode { get; set; }
        public string Type { get; set; }
        public int NumReports { get; set; }
        public int NumMentions { get; set; }
    }
}
