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


            var state = client.Cluster.Description.State;
            Console.WriteLine(state);
            




            /* THIS IS THE OLD WAY TO CONNECT TO MONGODB.. DIDn't WORK ON THIS WEB API.. IT WORKED ON OTHER WEB API 

         MongoServerSettings settings = new MongoServerSettings();
             settings.Server = new MongoServerAddress(
                 WebConfigurationManager.AppSettings["DataBaseURL"],
                 Convert.ToInt32(WebConfigurationManager.AppSettings["DataBasePort"]));

             MongoServer server = new MongoServer(settings);

             database = server.GetDatabase("Dataflow");   */
        }
    }
}