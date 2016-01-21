using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB;
using System.Web.Configuration;

namespace DataflowAnalyseWebApp.Controllers.Database {
    public class DBController {
        public IMongoDatabase database { get; private set; }

        public DBController() {
            string connectionString = WebConfigurationManager.AppSettings["DataBaseURL"];
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase("Dataflow");
        }
    }
}