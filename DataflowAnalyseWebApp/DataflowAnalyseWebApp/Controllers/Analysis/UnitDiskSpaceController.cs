using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using DataflowAnalyseWebApp.Models;

namespace DataflowAnalyseWebApp.Controllers
{
    public class UnitDiskSpaceController : ApiController
    {
        List<Monitoring> monitoringItems;
        public UnitDiskSpaceController()
        {
            monitoringItems = new List<Monitoring>();
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        private void GetDiskSpaceData()
        {
            DBController database = new DBController();
            IMongoQuery query = Query<Monitoring>.EQ(m => m.sensorType, "SystemInfo/AvailableDiskSpace");
            monitoringItems = database.database.GetCollection<Monitoring>("monitorings").Find(query).ToList();
        }

    }
}
