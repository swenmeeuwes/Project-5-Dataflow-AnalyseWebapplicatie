using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB;
using System.Web.Configuration;

namespace DataflowAnalyseWebApp.Controllers.Database
{
    public class DBController
    {
        public IMongoDatabase database { get; private set; }

        public DBController()
        {
            string connectionString = WebConfigurationManager.AppSettings["DataBaseURL"] + ":" + WebConfigurationManager.AppSettings["DataBasePort"];
            string simpleConnectionString = "mongodb://145.24.222.160:8010";
            var client = new MongoClient(simpleConnectionString);
            database = client.GetDatabase("Dataflow");
        }
    }
}