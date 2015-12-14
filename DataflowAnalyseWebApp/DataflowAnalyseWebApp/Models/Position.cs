using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class Position
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id; 
        public long unitId { get; set; }
        public DateTime dateTime { get; set; }
        public float rdX { get; set; }
        public float rdY { get; set; }
        public float latitudeGps { get; set; }
        public float longitudeGps { get; set; }
        public int speed { get; set; }
        public int course { get; set; }
        public int numSatellite { get; set; }
        public int hdop { get; set; }
        public string dopType { get; set; }
    }
}