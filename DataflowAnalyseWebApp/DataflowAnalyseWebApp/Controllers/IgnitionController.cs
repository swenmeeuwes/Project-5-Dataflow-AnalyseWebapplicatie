using DataflowAnalyseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace DataflowAnalyseWebApp.Controllers
{
    public class IgnitionController : ApiController
    {
        IMongoCollection<Event> ignitionCollection;
        
        Dictionary<long, Ignition> ignitionDictionary;
        List<Ignition> ignitionData;
        List<long> duplId;
        Dictionary<long, int> nodupl;
        const string eventPort = "Ignition";

        public IgnitionController()
        {
            DBController database = new DBController();
            ignitionCollection = database.database.GetCollection<Event>("events");
            ignitionDictionary = new Dictionary<long, Ignition>();
            nodupl = new Dictionary<long, int>();
            ignitionData = new List<Ignition>();
            
        }

        // GET api/ignition
        public IEnumerable<Ignition> Get()
        {
            GetIgnitionData();
            return ignitionData;
        }

        // GET api/ignition/5
        public IEnumerable<Ignition> Get(string beginDate, string endDate)
        {
            string[] beginDateSplitted = beginDate.Split('-');
            string[] endDateSplitted = endDate.Split('-');

            DateTime begin = new DateTime(Convert.ToInt32(beginDateSplitted[0]), Convert.ToInt32(beginDateSplitted[1]), Convert.ToInt32(beginDateSplitted[2]));
            DateTime end = new DateTime(Convert.ToInt32(endDateSplitted[0]), Convert.ToInt32(endDateSplitted[1]), Convert.ToInt32(endDateSplitted[2]));

            IMongoQuery query = Query<Ignition>.Where(m => m.dateTime >= begin && m.dateTime <= end); // Gebruikt position (p), van p check hij of het unitId en het opgegeven id hetzelfde zijn (EQ)
            return GetDatabase().GetCollection<Ignition>("events").Find(query);
        }

        private void GetIgnitionData()
        {
            var dbQuery = from events in ignitionCollection.AsQueryable()
                          where events.port == eventPort
                          select events;

            var countQuery = from events in ignitionCollection.AsQueryable()
                          where events.port == eventPort where events.portValue == 1
                          select events.unitId;

            removeDupl(countQuery);
            AddDataToList(dbQuery);
        }

        private void removeDupl(IQueryable<long> countQuery)
       {
           List<long> duplId = new List<long>();
            
            foreach (var item in countQuery)
           {
               duplId.Add(item);
           }

           var q = duplId.GroupBy(x => x)
               .Select(g => new { Value = g.Key, Count = g.Count() });

           foreach (var x in q)
           {
                nodupl.Add(x.Value, x.Count);
           }
       }

        private double calcAverage()
        {
            double average = nodupl.Values.Average();
            return average;            
        }

        private void AddDataToList(IQueryable<Event> dbQuery)
        {
            foreach (var ignitionItem in nodupl)
            {
                Ignition ignitionOutput = new Ignition();
                ignitionOutput.unitId = ignitionItem.Key;
                ignitionOutput.ignitionCount = ignitionItem.Value;
                ignitionOutput.ignitionAverage = calcAverage();
                ignitionOutput.dateTime = null;
                ignitionDictionary.Add(ignitionOutput.unitId, ignitionOutput);
            }
            ignitionData = ignitionDictionary.Values.ToList();
        }    
    }
}
