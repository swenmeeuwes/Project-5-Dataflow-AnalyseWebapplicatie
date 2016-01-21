using DataflowAnalyseWebApp.Controllers.Database;
using DataflowAnalyseWebApp.Models;
using DataflowAnalyseWebApp.Models.PositionModels;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace DataflowAnalyseWebApp.Controllers
{
    public class MaintenanceController : ApiController
    {
        const string analysisName = "Maintanence";
        MongoDatabase database;
        DocumentHandler documentHandler;
        public MaintenanceController()
        {
            database = new DatabaseController().database;
            documentHandler = new DocumentHandler();
        }
        // GET api/maintenance
        public IEnumerable<Maintenance> Get()
        {
            List<Maintenance> maintenanceList = new List<Maintenance>();
            if (documentHandler.Exists(analysisName))
                maintenanceList = documentHandler.Load<Maintenance>(analysisName).ToList();
            else
            {
                List<long> uniqueUnitIds = GetUniqueUnitIds();

                Task task = Task.Factory.StartNew(() => Parallel.ForEach(uniqueUnitIds, uniqueUnitId => maintenanceList.Add(GetMaintenanceFromUnitId(uniqueUnitId))));

                try
                {
                    Task.WaitAll(task);
                    documentHandler.Save(analysisName, maintenanceList);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return maintenanceList;
        }

        // GET api/maintenance/1426032000/1426118399
        public IEnumerable<Maintenance> Get(long beginTimestamp, long endTimestamp)
        {
            List<long> uniqueUnitIds = GetUniqueUnitIds();

            List <Maintenance> maintenanceList = new List<Maintenance>();
            Task task = Task.Factory.StartNew(() => Parallel.ForEach(uniqueUnitIds, uniqueUnitId => maintenanceList.Add(GetMaintenanceFromUnitId(uniqueUnitId, beginTimestamp, endTimestamp))));

            try
            {
                Task.WaitAll(task);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return maintenanceList;
        }

        // GET api/maintenance/15030001
        public Maintenance Get(long unitId)
        {
            return GetMaintenanceFromUnitId(unitId);
        }

        // GET api/maintenance/15030001/1426032000/1426118399
        public Maintenance Get(long unitId, long beginTimestamp, long endTimestamp)
        {
            return GetMaintenanceFromUnitId(unitId, beginTimestamp, endTimestamp);
        }

        [Route("api/maintenance/alert/{threshold}")]
        public IEnumerable<Maintenance> GetAlerts(long threshold)
        {
            List<Maintenance> maintenanceList = Get().ToList();
            return maintenanceList.Where((x) => x.kilometersTravelled >= threshold);
        }

        private Maintenance GetMaintenanceFromUnitId(long unitId)
        {
            List<Position> positions = database.GetCollection<Position>("positions").Find(Query<Position>.EQ(p => p.unitId, unitId)).ToList();

            Maintenance maintenance = new Maintenance();
            maintenance.unitId = unitId;

            double travelled = 0;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                GeoCoordinate position1 = new GeoCoordinate(positions[i].latitudeGps, positions[i].longitudeGps);
                GeoCoordinate position2 = new GeoCoordinate(positions[i + 1].latitudeGps, positions[i + 1].longitudeGps);
                travelled += position1.GetDistanceTo(position2) / 1000;
            }
            maintenance.kilometersTravelled = Math.Round(travelled, 2);
            return maintenance;
        }

        private Maintenance GetMaintenanceFromUnitId(long unitId, long beginTimestamp, long endTimestamp)
        {
            UnixTimestamp uts = new UnixTimestamp(beginTimestamp);
            DateTime utsDt = uts.ToDateTime();

            UnixTimestamp uts2 = new UnixTimestamp(endTimestamp);
            DateTime utsDt2 = uts2.ToDateTime();

            List<Position> positions = database.GetCollection<Position>("positions").Find(Query<Position>.EQ(p => p.unitId, unitId)).Where(p => p.dateTime.Ticks > new UnixTimestamp(beginTimestamp).ToDateTime().Ticks && p.dateTime.Ticks < new UnixTimestamp(endTimestamp).ToDateTime().Ticks).ToList();

            Maintenance maintenance = new Maintenance();
            maintenance.unitId = unitId;

            double travelled = 0;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                GeoCoordinate position1 = new GeoCoordinate(positions[i].latitudeGps, positions[i].longitudeGps);
                GeoCoordinate position2 = new GeoCoordinate(positions[i + 1].latitudeGps, positions[i + 1].longitudeGps);
                travelled += position1.GetDistanceTo(position2) / 1000;
            }
            maintenance.kilometersTravelled = Math.Round(travelled, 2);
            return maintenance;
        }

        private List<long> GetUniqueUnitIds()
        {
            List<long> uniqueUnitIds = new List<long>();
            if (documentHandler.Exists("UniqueUnitIds"))
                uniqueUnitIds = documentHandler.Load<long>("UniqueUnitIds").ToList();
            else
            {
                IEnumerable<BsonValue> bsonValues = database.GetCollection<Position>("positions").Distinct("unitId");
                uniqueUnitIds = BsonSerializer.Deserialize<List<long>>(bsonValues.ToJson());

                documentHandler.Save("UniqueUnitIds", uniqueUnitIds);
            }
            return uniqueUnitIds;
        }
    }
}
