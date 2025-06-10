using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Models
{
    public class IntelReports
    {
        public IntelReports(int reporterId, int targetId, string text, DateTime time)
        {
            ReporterId = reporterId;
            TargetId = targetId;
            Text = text;
            TimeStamp = time;
        }
        public int ID { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
