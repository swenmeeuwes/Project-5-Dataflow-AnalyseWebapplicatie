using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Web.Configuration;

namespace DataflowAnalyseWebApp.Controllers.Database
{
    public class DBController
    {
        public MongoDatabase database { get; private set; }

        public DBController()
        {
            MongoServerSettings settings = new MongoServerSettings();
            settings.Server = new MongoServerAddress(WebConfigurationManager.AppSettings["DataBaseURL"]);

            MongoServer server = new MongoServer(settings);

            database = server.GetDatabase("Dataflow");
        }
    }
}