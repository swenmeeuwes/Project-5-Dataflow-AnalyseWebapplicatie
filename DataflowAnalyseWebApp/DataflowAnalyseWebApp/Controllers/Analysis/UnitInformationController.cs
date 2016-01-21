using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using DataflowAnalyseWebApp.Models.PositionModels;

namespace DataflowAnalyseWebApp.Controllers {
    public class UnitInformationController : ApiController {

        IMongoCollection<Position> positionCollection;
        DBController database;

        private List<PositionBadUnit> positionData;

        public IMongoCollection<Position> PositionCollection
        {
            get
            {
                return positionCollection;
            }

            set
            {
                positionCollection = value;
            }
        }

        public UnitInformationController() {
            positionData = new List<PositionBadUnit>();
            
        }

        /// <summary>
        /// Retrieves only bad connections from the whole collection;
        /// </summary>
        public void RetrieveBadConnections() {
            positionData.Clear();
            var query = from position in PositionCollection.AsQueryable()
                        where position.hdop >= 5 || position.numSatellite <= 4
                        select position;
            DataToList(query);
            
        }
        
        /// <summary>
        /// Converts the data retrieved by the query to a list
        /// </summary>
        /// <param name="query"></param>
        public void DataToList(IQueryable<Position> query) {
            foreach (var item in query) {
                if (!positionData.Exists(x => x.unitId == item.unitId)) {
                    PositionBadUnit p = new PositionBadUnit();
                    p.unitId = item.unitId;
                    p.numSatellite = item.numSatellite;
                    p.hdop = item.hdop;
                    p.numOccurences = 1;
                    positionData.Add(p);
                }
                else {
                    PositionBadUnit p = positionData.Find(x => x.unitId == item.unitId);
                    p.numOccurences++;
                }
            }
        }

        public int FindAverage(List<PositionBadUnit> list) {
            int sumOfAllElements = 0;
            int numberOfElements = list.Count();
            foreach(PositionBadUnit p in list) {
                sumOfAllElements += p.numOccurences;
            }

            return sumOfAllElements / numberOfElements;
        }

        public int FindMaximum(List<PositionBadUnit> list) {
            int max = list[0].numOccurences;
            for(int i = 1; i < list.Count(); i++) {
                if (list[i].numOccurences > max) {
                    max = list[i].numOccurences;
                }
            }
            return max;
        }

        public int FindMinimum(List<PositionBadUnit> list) {
            int min = list[0].numOccurences;
            for (int i = 1; i < list.Count(); i++) {
                if (list[i].numOccurences < min) {
                    min = list[i].numOccurences = min;
                }
            }
            return min;
        }

        public int FindSum(List<PositionBadUnit> list) {
            int sum = 0;
            foreach(PositionBadUnit p in list) {
                sum += p.numOccurences;
            }
            return sum;
        }
        
        public IEnumerable<PositionBadUnit> Get() {
            database = new DBController();
            PositionCollection = database.database.GetCollection<Position>("positions");
            RetrieveBadConnections();
            positionData = positionData.OrderByDescending(x => x.numOccurences).ToList();
            return positionData.GetRange(0,5);
        }

        [Route("api/unitinformation/alert/{thresholdSatellite}/{thresholdHDOP}")]
        public IEnumerable<long> GetAlerts(int thresholdSatellite, int thresholdHDOP)
        {
            PositionCollection = database.database.GetCollection<Position>("positions");
            RetrieveBadConnections();
            positionData = positionData.OrderByDescending(x => x.numOccurences).ToList();

            List<PositionBadUnit> unitList = positionData.Distinct().ToList();
            var query = from item in unitList.AsQueryable()
                        where item.numSatellite <= thresholdSatellite || item.hdop >= thresholdHDOP
                        select item.unitId;

            return query.Distinct();
        }
    }
}