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


        public ConnectionController()
        {


        }


        public IEnumerable<Connection> Get()
        {
            List<Connection> connectionStatistics = calculateConnectionSpeed();
            List<Connection> connectionAverage = calculateAverage(connectionStatistics);
            return connectionAverage;
        }

        public List<Connection> calculateAverage(List<Connection> connectionStatistics)
        {
            List<Connection> connectionAverage = new List<Connection>();

            var average = connectionStatistics.Select(c => c).GroupBy(con => con.unitId).ToDictionary(f => f.Key, f => f.Average(con => con.connectionSpeed));
            foreach (KeyValuePair<long, double> entry in average) {

                Connection connection = new Connection();
                connection.unitId = entry.Key;
                
                connection.connectionSpeed = Math.Round(entry.Value, 0);
                connectionAverage.Add(connection);
            }

            return connectionAverage;
        }

        private List<Connection> calculateConnectionSpeed()
        {
            IMongoCollection<Event> eventCollection = getEventCollection();

            var ignitionOn = from events in eventCollection.AsQueryable()
                             where events.portValue == 1 && events.port == "Ignition"
                             select events;
            var connectionOn = from events in eventCollection.AsQueryable()
                               where events.portValue == 1 && events.port == "Connection"
                               select events;

            //TODO: ignitionOn - connectionOn (dichtsbijzijnde), ignitionOff - connectionOff (dichtsbijzijnde)
            //TODO: only subtract from same UnitId's
            List<Event> ignitionOnList = ignitionOn.ToList();
            List<Event> connectionOnList = connectionOn.ToList();

            List<Connection> connections = new List<Connection>();

            for (int x = 0; x < ignitionOnList.Count; x++)
            {
                Connection fastestTime = new Connection();
                fastestTime.connectionSpeed = -1;
                for (int y = 0; y < connectionOnList.Count; y++)
                {

                    if (ignitionOnList[x].unitId == connectionOnList[y].unitId)
                    {
                        long elapsedTicks = (connectionOnList[y].dateTime.Ticks - ignitionOnList[x].dateTime.Ticks);
                        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                        double elapsedSeconds = elapsedSpan.TotalSeconds;
                        Console.WriteLine(elapsedSeconds);
                        if (elapsedSeconds > 0 && (elapsedSeconds < fastestTime.connectionSpeed || fastestTime.connectionSpeed < 0))
                        {
                            fastestTime.connectionSpeed = elapsedSeconds;
                            fastestTime.unitId = connectionOnList[y].unitId;
                        }
                    }

                }
                if (fastestTime.connectionSpeed > 0) {
                    connections.Add(fastestTime);
                }
            }





            return connections;
        }


        private IMongoCollection<Event> getEventCollection()
        {

            DBController database = new DBController();
            IMongoCollection<Event> eventCollection = database.database.GetCollection<Event>("events");
            return eventCollection;
        }

    }
}
