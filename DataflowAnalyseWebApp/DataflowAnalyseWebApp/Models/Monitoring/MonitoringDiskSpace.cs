using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models.Monitoring
{
    public class MonitoringDiskSpace
    {
        public long unitId { get; set; }
        public DateTime endTime { get; set; }
        public double percentUsed { get; set; }
        public string diskSpaceStatus { get; set; }
    }
}