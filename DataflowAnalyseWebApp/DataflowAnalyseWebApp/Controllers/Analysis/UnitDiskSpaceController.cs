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
        //TODO add query by id, string status and percentage used
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
        public IEnumerable<Monitoring> Get(long id)
        {
            GetDiskSpaceById(id);
            return monitoringItems;
        }

        private void GetDiskSpaceData()
        {
            var query = from monitoring in monitoringsCollection.AsQueryable()
                        where monitoring.sensorType == sensorType
                        select monitoring;

            monitoringItems.Clear();
            foreach (var diskSpaceItem in query)
            {
                monitoringItems.Add(diskSpaceItem);
            }
        }

        // GET api/UnitDiskSpace/GetFewDiskSpace
        public IEnumerable<Monitoring> GetFewDiskSpace()
        {
            GetFewDiskSpaceData();
            return monitoringItems;
        }


        private void GetFewDiskSpaceData()
        {
            var query = from monitoring in monitoringsCollection.AsQueryable()
                        where monitoring.sensorType == sensorType && (monitoring.maxValue * 0.75) >= monitoring.sumValue
                        select monitoring;

            monitoringItems.Clear();
            foreach (var diskSpaceItem in query)
            {
                monitoringItems.Add(diskSpaceItem);
            }
        }
        private void GetDiskSpaceById(long id)
        {
            var query = from monitoring in monitoringsCollection.AsQueryable()
                        where monitoring.sensorType == sensorType && monitoring.unitId == id
                        select monitoring;
        }

        private void GetSmallDiskSpace()
        {
            var query = from monitoring in monitoringsCollection.AsQueryable()
                        where monitoring.sensorType == sensorType && (monitoring.maxValue * 0.25) <= monitoring.sumValue
                        select monitoring;
        }

        

    }
}
