using DataflowAnalyseWebApp.Controllers.Database;
using DataflowAnalyseWebApp.Models;
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
        // GET api/values
        public IEnumerable<Maintenance> Get()
        {
            MongoDatabase database = new DatabaseController().database;

            IEnumerable<BsonValue> bsonValues = database.GetCollection<Position>("positions").Distinct("unitId");
            List<long> uniqueUnitIds = BsonSerializer.Deserialize<List<long>>(bsonValues.ToJson());

            List<Maintenance> maintenanceList = new List<Maintenance>();
            Task task = Task.Factory.StartNew(() => Parallel.ForEach(uniqueUnitIds, uniqueUnitId => maintenanceList.Add(GetMaintenanceFromUnitId(database, uniqueUnitId))));

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

        public IEnumerable<Maintenance> Get(long beginTimestamp, long endTimestamp)
        {
            MongoDatabase database = new DatabaseController().database;

            IEnumerable<BsonValue> bsonValues = database.GetCollection<Position>("positions").Distinct("unitId");
            List<long> uniqueUnitIds = BsonSerializer.Deserialize<List<long>>(bsonValues.ToJson());

            List<Maintenance> maintenanceList = new List<Maintenance>();
            Task task = Task.Factory.StartNew(() => Parallel.ForEach(uniqueUnitIds, uniqueUnitId => maintenanceList.Add(GetMaintenanceFromUnitId(database, uniqueUnitId, beginTimestamp, endTimestamp))));

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

        // GET api/maintenance/5
        public Maintenance Get(long unitId)
        {
            return GetMaintenanceFromUnitId(unitId);
        }

        private List<T> GetCollection<T>(string collectionName, IMongoQuery query)
        {
            MongoDatabase database = new DatabaseController().database;
            return database.GetCollection<T>(collectionName).Find(query).ToList();
        }
        private List<T> GetCollection<T>(MongoDatabase database, string collectionName, IMongoQuery query)
        {
            return database.GetCollection<T>(collectionName).Find(query).ToList();
        }

        private Maintenance GetMaintenanceFromUnitId(long unitId)
        {
            List<Position> positions = GetCollection<Position>("positions", Query<Position>.EQ(p => p.unitId, unitId));

            Maintenance maintenance = new Maintenance();
            maintenance.unitId = unitId;

            double travelled = 0;
            for (int i = 0; i < positions.Count - 1; i ++)
            {
                GeoCoordinate position1 = new GeoCoordinate(positions[i].latitudeGps, positions[i].longitudeGps);
                GeoCoordinate position2 = new GeoCoordinate(positions[i + 1].latitudeGps, positions[i + 1].longitudeGps);
                travelled += position1.GetDistanceTo(position2) / 1000;
            }
            maintenance.kilometersTravelled = Math.Round(travelled, 2);
            return maintenance;
        }

        private Maintenance GetMaintenanceFromUnitId(MongoDatabase database, long unitId)
        {
            List<Position> positions = GetCollection<Position>(database, "positions", Query<Position>.EQ(p => p.unitId, unitId));

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

        private Maintenance GetMaintenanceFromUnitId(MongoDatabase database, long unitId, long beginTimestamp, long endTimestamp)
        {
            UnixTimestamp uts = new UnixTimestamp(beginTimestamp);
            DateTime utsDt = uts.ToDateTime();

            UnixTimestamp uts2 = new UnixTimestamp(endTimestamp);
            DateTime utsDt2 = uts2.ToDateTime();

            List<Position> positions = GetCollection<Position>(database, "positions", Query<Position>.EQ(p => p.unitId, unitId)).Where(p => p.dateTime.Ticks > new UnixTimestamp(beginTimestamp).ToDateTime().Ticks && p.dateTime.Ticks < new UnixTimestamp(endTimestamp).ToDateTime().Ticks).ToList();

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
    }
}
