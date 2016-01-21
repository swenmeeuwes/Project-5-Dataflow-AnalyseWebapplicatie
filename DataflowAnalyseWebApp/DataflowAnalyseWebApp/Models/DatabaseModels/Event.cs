using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class Event
    {
        [JsonIgnore]
        public ObjectId _id { get; set; }
        public DateTime dateTime { get; set; }
        public long unitId { get; set; }
        public string port { get; set; }
        public int portValue { get; set; }
    }
}
