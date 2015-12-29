using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class Ignition
    {
        public long unitId { get; set; }
        public int ignitionCount { get; set; }
        public int ignitionAverage { get; set; }
    }
}