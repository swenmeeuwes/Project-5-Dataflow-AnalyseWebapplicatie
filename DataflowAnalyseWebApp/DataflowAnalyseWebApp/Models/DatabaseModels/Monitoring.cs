﻿using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models.MonitoringModels
{
    public class Monitoring
    {
        [JsonIgnore]
        public ObjectId _id { get; set; }
        public long unitId { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
        public string sensorType { get; set; }
        public double minValue { get; set; }
        public double maxValue { get; set; }
        public double sumValue { get; set; }
    }
}