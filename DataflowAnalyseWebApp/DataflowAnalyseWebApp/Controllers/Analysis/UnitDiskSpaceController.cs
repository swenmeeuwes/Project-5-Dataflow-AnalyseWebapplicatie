using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using DataflowAnalyseWebApp.Models.Monitoring;

namespace DataflowAnalyseWebApp.Controllers
{
    public class UnitDiskSpaceController : ApiController
    {
        //TODO output must be small dataset. input = monitoring model.. output = monitoringDiskSpace
        private IMongoCollection<Monitoring> monitoringsCollection;
        private readonly string sensorType = "SystemInfo/AvailableDiskSpace";

        public UnitDiskSpaceController()
        {
            DBController database = new DBController();
            monitoringsCollection = database.database.GetCollection<Monitoring>("monitorings");
            monitoringItems = new List<Monitoring>();
        }

        List<Monitoring> monitoringItems;
        // GET api/UnitDiskSpace
        public IEnumerable<Monitoring> Get()
        {
            GetDiskSpaceData();
            return monitoringItems;
        }
        // GET api/UnitDiskSpace/5
        public IEnumerable<Monitoring> Get(long unitId)
        {
            GetDiskSpaceById(unitId);
            return monitoringItems;
        }
        private void GetDiskSpaceData()
        {
            var query = from monitoring in monitoringsCollection.AsQueryable()
                        where monitoring.sensorType == sensorType
                        select monitoring;

            PutDataToList(query);
        }

        private void GetDiskSpaceById(long unitId)
        {
            var query = from monitoring in monitoringsCollection.AsQueryable()
                        where monitoring.sensorType == sensorType && monitoring.unitId == unitId
                        select monitoring;
            PutDataToList(query);
        }

        private void PutDataToList(IQueryable<Monitoring> query)
        {
            monitoringItems.Clear();
            foreach (var diskSpaceItem in query)
            {
                diskSpaceItem.percentUsed = Math.Round((diskSpaceItem.maxValue / diskSpaceItem.sumValue) * 100, 2);

                if (diskSpaceItem.percentUsed <= 25)
                {
                    diskSpaceItem.diskSpaceStatus = "Empty";
                }
                else if (diskSpaceItem.percentUsed > 25 && diskSpaceItem.percentUsed < 75)
                {
                    diskSpaceItem.diskSpaceStatus = "Half full";
                }
                else if (diskSpaceItem.percentUsed >= 75)
                {
                    diskSpaceItem.diskSpaceStatus = "Allmost full";
                }else if (diskSpaceItem.percentUsed >= 85)
                {
                    diskSpaceItem.diskSpaceStatus = "Full";
                }
                
                monitoringItems.Add(diskSpaceItem);
            }
        }
    }
}
