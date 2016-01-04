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
            MongoDatabase database = new DBController2().database;
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

        // GET api/maintenance/5
        public Maintenance Get(long unitId)
        {
            return GetMaintenanceFromUnitId(unitId);
        }

        private Maintenance GetMaintenanceFromUnitId(long unitId)
        {
            MongoDatabase database = new DBController2().database;
            IMongoQuery query = Query<Position>.EQ(p => p.unitId, unitId);
            List<Position> positions = database.GetCollection<Position>("positions").Find(query).ToList();


            Maintenance maintenance = new Maintenance();
            maintenance.unitId = unitId;

            double travelled = 0;
            for (int i = 0; i < positions.Count - 1; i ++)
            {
                GeoCoordinate position1 = new GeoCoordinate(positions[i].latitudeGps, positions[i].longitudeGps);
                GeoCoordinate position2 = new GeoCoordinate(positions[i + 1].latitudeGps, positions[i + 1].longitudeGps);
                travelled += position1.GetDistanceTo(position2) / 1000;
                //travelled += CalcDistance(positions[i].latitudeGps, positions[i].longitudeGps, positions[i + 1].latitudeGps, positions[i + 1].longitudeGps);
            }
            maintenance.kilometersTravelled = Math.Round(travelled, 2);
            return maintenance;
        }

        private Maintenance GetMaintenanceFromUnitId(MongoDatabase database, long unitId)
        {
            IMongoQuery query = Query<Position>.EQ(p => p.unitId, unitId);
            List<Position> positions = database.GetCollection<Position>("positions").Find(query).ToList();


            Maintenance maintenance = new Maintenance();
            maintenance.unitId = unitId;

            double travelled = 0;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                travelled += CalcDistance(positions[i].latitudeGps, positions[i].longitudeGps, positions[i + 1].latitudeGps, positions[i + 1].longitudeGps);
            }
            maintenance.kilometersTravelled = Math.Round(travelled, 2);
            return maintenance;
        }
        [Obsolete]
        private double CalcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(DegToRad(lat1)) * Math.Sin(DegToRad(lat2)) + Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) * Math.Cos(DegToRad(theta));
            dist = Math.Acos(dist);
            dist = RadToDeg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            return (dist);
        }
        [Obsolete]
        private double DegToRad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        [Obsolete]
        private double RadToDeg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }

    internal class DBController2
    {
        public MongoDatabase database { get; private set; }

        public DBController2()
        {
            MongoServerSettings settings = new MongoServerSettings();
            settings.Server = new MongoServerAddress("145.24.222.160", 8010);

            MongoServer server = new MongoServer(settings);

            database = server.GetDatabase("Dataflow");
        }
    }
}
