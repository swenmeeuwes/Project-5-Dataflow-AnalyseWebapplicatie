using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class Monitoring
    {
        public ObjectId _id { get; set; }
        public long unitId { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
        public string sensorType { get; set; }
        public double minValue { get; set; }
        public double maxValue { get; set; }
        public double sumValue { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} ", unitId, beginTime, endTime, sensorType, minValue, maxValue, sumValue);
        }
    }
}