using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models.Position {
    public class PositionBadUnit {

        public long unitId { get; set; }
        public int numSatellite { get; set; }
        public int hdop { get; set; }
        public int numOccurences { get; set; }
    }
}