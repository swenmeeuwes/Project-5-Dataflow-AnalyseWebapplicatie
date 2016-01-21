using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using DataflowAnalyseWebApp.Controllers.Database;
using DataflowAnalyseWebApp.Models.MonitoringModels;

namespace DataflowAnalyseWebApp.Controllers
{
    public class UnitDiskSpaceController : ApiController
    {     
        IMongoCollection<Monitoring> monitoringsCollection;
        const string sensorType = "SystemInfo/AvailableDiskSpace";
        const int diskSpaceCapacity = 1300000000;
        List<MonitoringDiskSpace> monitoringItems;

        public UnitDiskSpaceController()
        {
            DBController database = new DBController();
            monitoringsCollection = database.database.GetCollection<Monitoring>("monitorings");
            monitoringItems = new List<MonitoringDiskSpace>();
        }

        [Route("api/unitdiskspace/alert/{threshold}")]
        public IEnumerable<long> GetAlerts(long threshold)
        {
            List<MonitoringDiskSpace> monitoringList = Get().Distinct().ToList();
            var query = from item in monitoringList.AsQueryable()
                   where item.percentUsed >= threshold
                   select item.unitId;

            return query.Distinct();
        }

        // GET api/UnitDiskSpace
        public IEnumerable<MonitoringDiskSpace> Get()
        {
            GetDiskSpaceData();
            return monitoringItems;
        }
        // GET api/UnitDiskSpace/5
        public IEnumerable<MonitoringDiskSpace> Get(long unitId)
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
                MonitoringDiskSpace diskSpaceOutput = new MonitoringDiskSpace();
                diskSpaceOutput.unitId = diskSpaceItem.unitId;
                diskSpaceOutput.endTime = diskSpaceItem.endTime;
                diskSpaceOutput.percentUsed = Math.Round((diskSpaceItem.maxValue / diskSpaceCapacity) * 100, 2);

                if (diskSpaceOutput.percentUsed <= 25)
                {
                    diskSpaceOutput.diskSpaceStatus = "Empty";
                }
                else if (diskSpaceOutput.percentUsed > 25 && diskSpaceOutput.percentUsed < 75)
                {
                    diskSpaceOutput.diskSpaceStatus = "Half full";
                }
                else if (diskSpaceOutput.percentUsed >= 75 && diskSpaceOutput.percentUsed < 90)
                {
                    diskSpaceOutput.diskSpaceStatus = "Almost full";
                }else if (diskSpaceOutput.percentUsed >= 90)
                {
                    diskSpaceOutput.diskSpaceStatus = "Full";
                }
                
                monitoringItems.Add(diskSpaceOutput);
            }
        }
    }
}
