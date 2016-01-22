using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using DataflowAnalyseWebApp.Models.Monitoring;
using DataflowAnalyseWebApp.Controllers.Database;

namespace DataflowAnalyseWebApp.Controllers
{
    public class UnitDiskSpaceController : ApiController
    {     
        IMongoCollection<Monitoring> monitoringsCollection;
        const string sensorType = "SystemInfo/AvailableDiskSpace";
        const int diskSpaceCapacity = 1300000000;
        List<MonitoringDiskSpace> monitoringItems;
        List<DiskSpaceStatus> diskSpaceStatusItems;

        public UnitDiskSpaceController()
        {
            DBController database = new DBController();
            monitoringsCollection = database.database.GetCollection<Monitoring>("monitorings");
            monitoringItems = new List<MonitoringDiskSpace>();
            diskSpaceStatusItems = new List<DiskSpaceStatus>();
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

        //Get api/UnitDiskSpace/id/status
        public IEnumerable<DiskSpaceStatus> Get(long unitId, string status)
        {
            GetDiskSpaceData();
            GetDiskSpaceStatus();

            return diskSpaceStatusItems;
        }

        private void GetDiskSpaceStatus()
        {

            var query = from monitoring in monitoringItems.AsQueryable()
                        group monitoring by monitoring.diskSpaceStatus into status
                        select new {status.Key, Count = status.Count() };

            foreach (var statusItem in query)
            {
                DiskSpaceStatus statusCount = new DiskSpaceStatus();
                statusCount.diskSpaceStatus = statusItem.Key;
                statusCount.statusAmount = statusItem.Count;

                diskSpaceStatusItems.Add(statusCount);
            }                        
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
                        where monitoring.sensorType == sensorType
                        orderby monitoring.unitId
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
                    diskSpaceOutput.diskSpaceStatus = "Allmost full";
                }else if (diskSpaceOutput.percentUsed >= 90)
                {
                    diskSpaceOutput.diskSpaceStatus = "Full";
                }
                
                monitoringItems.Add(diskSpaceOutput);
            }
        }
        private void PutDataToListSecond(IQueryable<Monitoring> query)
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
                    diskSpaceOutput.diskSpaceStatus = "Allmost full";
                }
                else if (diskSpaceOutput.percentUsed >= 90)
                {
                    diskSpaceOutput.diskSpaceStatus = "Full";
                }

                monitoringItems.Add(diskSpaceOutput);
            }
        }
    }
}
