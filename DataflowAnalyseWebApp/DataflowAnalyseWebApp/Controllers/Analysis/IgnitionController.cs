using DataflowAnalyseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using MongoDB.Driver.Builders;

namespace DataflowAnalyseWebApp.Controllers
{
    public class IgnitionController : ApiController
    {
        IMongoCollection<Event> ignitionCollection;
        private DatabaseController dbcontroller;
        
        Dictionary<long, Ignition> ignitionDictionary;
        List<Ignition> ignitionData;
        List<long> duplId;
        Dictionary<long, int> nodupl;
        const string eventPort = "Ignition";

        public IgnitionController()
        {           
            ignitionDictionary = new Dictionary<long, Ignition>();
            nodupl = new Dictionary<long, int>();
            ignitionData = new List<Ignition>();           
        }

        //[Route("api/ignition/alert/{threshold}")]
        //public IEnumerable<long> GetAlerts(int threshold)
        //{
        //    List<Ignition> unitList = Get().Distinct().ToList();
        //    var query = from item in unitList.AsQueryable()
        //                where 
        //                select item.unitId;

        //    return query.Distinct();
        //}

        // GET api/ignition
        public IEnumerable<Ignition> Get()
        {            
            return ignitionData;
        }

        public IEnumerable<Ignition> Get(string beginDate, string endDate)
        {
            string[] beginDateSplitted = beginDate.Split('-');
            string[] endDateSplitted = endDate.Split('-');

            DBController database = new DBController();
            dbcontroller = new DatabaseController();
            ignitionCollection = database.database.GetCollection<Event>("events");

            DateTime begin = new DateTime(Convert.ToInt32(beginDateSplitted[0]), Convert.ToInt32(beginDateSplitted[1]), Convert.ToInt32(beginDateSplitted[2]));
            DateTime end = new DateTime(Convert.ToInt32(endDateSplitted[0]), Convert.ToInt32(endDateSplitted[1]), Convert.ToInt32(endDateSplitted[2]));

            IMongoQuery query = Query<Event>.Where(m => m.dateTime >= begin && m.dateTime <= end && m.port == eventPort && m.portValue == 1); // Gebruikt event (e), van e check hij of het unitId en het opgegeven id hetzelfde zijn (EQ)
            List<Event> EventList = dbcontroller.database.GetCollection<Event>("events").Find(query).ToList();
            RemoveDuplicatesFromList(EventList);
            AddData(EventList);
            return ignitionData;
        }

        
        public Dictionary<long, int> RemoveDuplicatesFromList(List<Event> EventList)
        {
            List<long> duplicateIds = new List<long>();

            foreach (var item in EventList)
            {
                duplicateIds.Add(item.unitId);
            }

            var q = duplicateIds.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() });

            foreach (var x in q)
            {
                nodupl.Add(x.Value, x.Count);
            }
            return nodupl;
        }


        public double CalculateAverage(Dictionary<long, int> nodupl)
        {
             double average = nodupl.Values.Average();
            return average;
            
        }

        private void AddData(List<Event> dbList)
        {
            foreach (var ignitionItem in nodupl)
            {
                Ignition ignitionOutput = new Ignition();
                ignitionOutput.unitId = ignitionItem.Key;
                ignitionOutput.ignitionCount = ignitionItem.Value;
                ignitionOutput.ignitionAverage = CalculateAverage(nodupl);
                ignitionDictionary.Add(ignitionOutput.unitId, ignitionOutput);
            }
            ignitionData = ignitionDictionary.Values.ToList();
        }
 
    }
}
