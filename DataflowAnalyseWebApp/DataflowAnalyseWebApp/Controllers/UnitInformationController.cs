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

namespace DataflowAnalyseWebApp.Controllers {
    public class UnitInformationController : ApiController {

        IMongoCollection<Position> positionCollection;

        private List<Position> positionData;
        private List<Position> filteredData;

        public UnitInformationController() {
            DBController database = new DBController();
            positionCollection = database.database.GetCollection<Position>("positions");
            positionData = new List<Position>();
        }

        /// <summary>
        /// Returns a dictionary containing the unitID as a key and the number of times it occurs as value
        /// </summary>
        /// <returns>Dictionary containing number of occurences per unitID</returns>
        public Dictionary<long, int> CheckOccurence() {
            Dictionary<long, int> result = new Dictionary<long, int>();
            foreach (Position unit in filteredData) {
                if (result.ContainsKey(unit.unitId)) {
                    result[unit.unitId] += 1;
                }
                else {
                    result.Add(unit.unitId, 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Removes all 'good' connections from the data
        /// </summary>
        /// <returns>List containing only bad connections</returns>
        public List<Position> GetBadConnections() {
            foreach (Position unit in allData) {
                if (unit.numSatellite < 3 && unit.hdop > 5) {
                    filteredData.Add(unit);
                }
            }
            return filteredData;
        }

        /// <summary>
        /// Makes a list of the unitIDs which occur more than x times in the bad connections list
        /// </summary>
        /// <returns>List of unitIDs with frequent bad connections</returns>
        public List<long> GetFrequenceBadConnections() {
            List<long> badUnits = new List<long>();
            Dictionary<long, int> occurences = CheckOccurence();
            foreach(var pair in occurences) {
                if(pair.Value >= 100) {
                    badUnits.Add(pair.Key);
                }
            }
            return badUnits;
        }
    }
}