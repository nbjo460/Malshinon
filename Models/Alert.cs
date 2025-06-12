using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Models
{
    public class Alert
    {
        public int ID { get; set; }
        public int TargetId { get; set; } = -1;
        public DateTime EndWindow { get; set; }
        public DateTime StartWindow { get; set; }
        public string Reason { get; set; }
        public Alert()
        {

        }
        public override string ToString()
        {
            return $"-------ALERT!!!!!-------\n" +
                   $"|  {EndWindow}   |\n" +
                   $"|  {StartWindow} |\n" +
                   $"------ALERT!!!!!-------";
        }
    }
}
