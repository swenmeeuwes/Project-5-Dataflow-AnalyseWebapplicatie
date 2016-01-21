using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models.MonitoringModels
{
    public class MonitoringDiskSpace
    {
        public long unitId { get; set; }
        public DateTime endTime { get; set; }
        public double percentUsed { get; set; }
        public string diskSpaceStatus { get; set; }

        public MonitoringDiskSpace() { }

        public MonitoringDiskSpace(long unitId, DateTime endTime, double percentUsed, string diskSpaceStatus) {
            this.unitId = unitId;
            this.endTime = endTime;
            this.percentUsed = percentUsed;
            this.diskSpaceStatus = diskSpaceStatus;

        }
    }
}