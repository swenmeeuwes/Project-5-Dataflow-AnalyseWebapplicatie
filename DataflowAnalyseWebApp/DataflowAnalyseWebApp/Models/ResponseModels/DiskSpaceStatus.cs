using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataflowAnalyseWebApp.Models.ResponseModels
{
    public class DiskSpaceStatus
    {
        public string diskSpaceStatus { get; set; }
        public long statusAmount { get; set; }
    }
}