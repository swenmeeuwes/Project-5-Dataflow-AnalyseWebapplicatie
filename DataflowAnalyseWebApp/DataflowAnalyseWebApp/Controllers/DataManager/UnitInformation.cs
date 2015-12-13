using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using DataflowAnalyseWebApp.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

namespace DataflowAnalyseWebApp
{
    public class UnitInformation
    {

        private List<Position> allData;
        private List<Position> filteredData;

        public UnitInformation()
        {
            allData = GetData();
            filteredData = new List<Position>();
        }

        /// <summary>
        /// Downloads the JSON file from the webservice
        /// </summary>
        /// <returns>List of Position object, converted from the JSON file</returns>
        public List<Position> GetData()
        {
            WebClient client = new WebClient();
            string rawData = client.DownloadString("http://145.24.222.160/DataFlowWebservice/api/positions");
            string jsonArray = rawData.Substring(rawData.IndexOf("[{"));
            jsonArray = jsonArray.Remove(jsonArray.Length - 1);

            List<Position> data = JsonConvert.DeserializeObject<List<Position>>(jsonArray);
            return data;
        }

        /// <summary>
        /// Returns a dictionary containing the unitID as a key and the number of times it occurs as value
        /// </summary>
        /// <returns>Dictionary containing number of occurences per unitID</returns>
        public Dictionary<long, int> CheckOccurence()
        {
            Dictionary<long, int> result = new Dictionary<long, int>();
            foreach (Position unit in filteredData)
            {
                if (result.ContainsKey(unit.unitId))
                {
                    result[unit.unitId] += 1;
                }
                else
                {
                    result.Add(unit.unitId, 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Removes all 'good' connections from the data
        /// </summary>
        /// <returns>List containing only bad connections</returns>
        public List<Position> GetBadConnections()
        {
            foreach (Position unit in allData)
            {
                if (unit.numSatellite > 3 && unit.hdop < 5)
                {
                    filteredData.Add(unit);
                }
            }
            return filteredData;
        }

        /// <summary>
        /// Makes a list of the unitIDs which occur more than x times in the bad connections list
        /// </summary>
        /// <returns>List of unitIDs with frequent bad connections</returns>
        public List<long> GetFrequenceBadConnections()
        {
            List<long> badUnits = new List<long>();
            Dictionary<long, int> occurences = CheckOccurence();
            foreach (var pair in occurences)
            {
                if (pair.Value >= 100)
                {
                    badUnits.Add(pair.Key);
                }
            }
            return badUnits;
        }
    }
}
