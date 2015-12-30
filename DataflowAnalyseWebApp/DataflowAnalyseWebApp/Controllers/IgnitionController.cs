using DataflowAnalyseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;

namespace DataflowAnalyseWebApp.Controllers
{
    public class IgnitionController : ApiController
    {
        IMongoCollection<Event> ignitionCollection;
        
        Dictionary<long, Ignition> ignitionDictionary;
        List<Ignition> ignitionData;
        const string eventPort = "Ignition";

        public IgnitionController()
        {
            DBController database = new DBController();
            ignitionCollection = database.database.GetCollection<Event>("events");
            ignitionDictionary = new Dictionary<long, Ignition>();
            ignitionData = new List<Ignition>();
        }

        // GET api/ignition
        public IEnumerable<Ignition> Get()
        {
            GetIgnitionData();
            return ignitionData;
        }

        // GET api/ignition/5
        public string Get(long id)
        {
            return "value";
        }

        private void GetIgnitionData()
        {
            var dbQuery = from events in ignitionCollection.AsQueryable()
                          where events.port == eventPort
                          select events;

            AddDataToList(dbQuery);
        }

        private void AddDataToList(IQueryable<Event> dbQuery)
        {
            foreach (var ignitionItem in dbQuery)
            {
                Ignition ignitionOutput = new Ignition();

                ignitionOutput.unitId = ignitionItem.unitId;

                if (!ignitionDictionary.ContainsKey(ignitionOutput.unitId))
                {
                    ignitionDictionary.Add(ignitionOutput.unitId, ignitionOutput);
                } 
                
            }
            ignitionData = ignitionDictionary.Values.ToList();
        }
    }
}
