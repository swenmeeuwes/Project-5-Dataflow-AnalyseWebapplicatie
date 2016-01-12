using DataflowAnalyseWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace DataflowAnalyseWebApp.Controllers
{
    public class ConnectionController : ApiController
    {
        IMongoCollection<Event> ignitionCollection;
        

        public ConnectionController()
        {


        }

        
        public IEnumerable<Connection> Get()
        {
            List<Connection> connectionStatistics = calculateConnectionSpeed();
            return connectionStatistics;
        }

        private List<Connection> calculateConnectionSpeed()
        {
            IMongoCollection<Event> eventCollection = getEventCollection();

            var ignitionOff = from events in eventCollection.AsQueryable()
                          where events.portValue == 0 && events.port == "Ignition"
                          select events;
            var ignitionOn = from events in eventCollection.AsQueryable()
                              where events.portValue == 1 && events.port == "Ignition"
                              select events;
            var connectionOff = from events in eventCollection.AsQueryable()
                              where events.portValue == 0 && events.port == "Connection"
                              select events;
            var connectionOn = from events in eventCollection.AsQueryable()
                              where events.portValue == 1 && events.port == "Connection"
                              select events;

            //TODO: ignitionOn - connectionOn (dichtsbijzijnde), ignitionOff - connectionOff (dichtsbijzijnde)
            //TODO: only subtract from same UnitId's
            List<Event> ignitionOffList = ignitionOff.OrderBy(o => o.dateTime).ToList();
            List<Event> ignitionOnList = ignitionOn.OrderBy(o => o.dateTime).ToList();
            List<Event> connectionOffList = connectionOff.OrderBy(o => o.dateTime).ToList();
            List<Event> connectionOnList = connectionOn.OrderBy(o => o.dateTime).ToList();

            List<Connection> connections = new List<Connection>();

            for (int x = 0; x < ignitionOffList.Count; x++) {
                if (ignitionOffList[x].unitId == connectionOffList[x].unitId) {
                    Connection connection = new Connection();
                    connection.connectionSpeed = (connectionOffList[x].dateTime - ignitionOffList[x].dateTime).Ticks;
                    connection.unitId = ignitionOffList[x].unitId;
                    connections.Add(connection);
                }
            }

            



            return connections;
        }


        private IMongoCollection<Event> getEventCollection() {

            DBController database = new DBController();
            IMongoCollection<Event> eventCollection = database.database.GetCollection<Event>("events");
            return eventCollection;
        }

    }
}
