using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models.PositionModels {
    public class PositionBadUnit {

        public long unitId { get; set; }
        public int numSatellite { get; set; }
        public int hdop { get; set; }
        public int numOccurences { get; set; }

        public PositionBadUnit() {}

        public PositionBadUnit(long unitId, int numSatellite, int hdop, int numOccurences) {
            this.unitId = unitId;
            this.numSatellite = numSatellite;
            this.hdop = hdop;
            this.numOccurences = numOccurences;
        }
    }
}