using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace DataflowAnalyseWebApp.Controllers.Database
{
    public class DBController
    {
        public MongoDatabase database { get; private set; }

        public DBController()
        {
            MongoServerSettings settings = new MongoServerSettings();
            settings.Server = new MongoServerAddress("145.24.222.160", 8010);

            MongoServer server = new MongoServer(settings);

            database = server.GetDatabase("Dataflow");
        }
    }
}