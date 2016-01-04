using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DataflowAnalyseWebApp.Controllers.Database
{
    [Obsolete]
    public class DatabaseController
    {
        public MongoDatabase database { get; private set; }

        public DatabaseController()
        {
            MongoServerSettings settings = new MongoServerSettings();
            settings.Server = new MongoServerAddress(WebConfigurationManager.AppSettings["DatabaseIp"], Convert.ToInt32(WebConfigurationManager.AppSettings["DatabasePort"]));

            MongoServer server = new MongoServer(settings);

            database = server.GetDatabase("Dataflow");
        }
    }
}