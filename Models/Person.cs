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
        public string firstName { get; set; }
        public string lastName {get; set;}
        public string secretCode { get; set; }
        public string type { get; set; }
        public int numReports { get; set; }
        public int numMentions { get; set; }
    }
}
