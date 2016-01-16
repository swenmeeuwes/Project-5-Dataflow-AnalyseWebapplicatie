using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataflowAnalyseWebApp.Controllers.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using DataflowAnalyseWebApp.Models.PositionModels;

namespace DataflowAnalyseWebApp.Controllers {
    public class UnitInformationController : ApiController {

        IMongoCollection<Position> positionCollection;

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
            DBController database = new DBController();
            PositionCollection = database.database.GetCollection<Position>("positions");
            positionData = new List<PositionBadUnit>();
        }

        /// <summary>
        /// Retrieves only bad connections from the whole collection;
        /// </summary>
        public void GetBadConnections() {
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
        
        public IEnumerable<PositionBadUnit> Get() {
            GetBadConnections();
            positionData = positionData.OrderByDescending(x => x.numOccurences).ToList();
            return positionData.GetRange(0,5);
        }      
    }
}