using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models.Monitoring
{
    public class DiskSpaceStatus
    {        
        public string diskSpaceStatus { get; set; }
        public long statusAmount { get; set; }
    }
}