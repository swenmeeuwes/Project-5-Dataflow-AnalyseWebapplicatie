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
            GetDiskSpaceData();
            IterateDiskSpaceObjects();
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        private void GetDiskSpaceData()
        {
            
            DBController database = new DBController();
            var collection = database.database.GetCollection<Monitoring>("monitorings");
            var query = from c in collection.AsQueryable<Monitoring>()
                        where c.sensorType == "SystemInfo/AvailableDiskSpace"
                        select c;

                foreach (var diskSpaceItem in query)
                {
                    System.Diagnostics.Debug.WriteLine(diskSpaceItem.unitId + " " + diskSpaceItem.sensorType + " " + diskSpaceItem.sumValue + " " + diskSpaceItem.beginTime + " " + diskSpaceItem.endTime);
                }           
        }
        public void IterateDiskSpaceObjects()
        {
            foreach (var diskSpaceItem in monitoringItems)
            {
                Console.WriteLine(diskSpaceItem.unitId + " " +diskSpaceItem.sensorType);
            }
            Console.Read();
        }

    }
}
