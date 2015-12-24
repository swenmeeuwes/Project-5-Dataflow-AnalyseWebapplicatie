using MongoDB.Bson;
using System;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class Event
    {
        public DateTime dateTime { get; set; }
        public long unitId { get; set; }
        public string port { get; set; }
        public int portValue { get; set; }
    }
}
