using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class Monitoring
    {
        public long unitId { get; set; }
        public string beginTime { get; set; }
        public string endTime { get; set; }
        public string sensorType { get; set; }
        public double minValue { get; set; }
        public double maxValue { get; set; }
        public double sumValue { get; set; }
    }
}