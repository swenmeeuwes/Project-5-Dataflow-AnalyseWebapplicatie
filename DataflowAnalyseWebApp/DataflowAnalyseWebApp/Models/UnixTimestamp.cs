using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class UnixTimestamp
    {
        long unixTimestamp;

        public UnixTimestamp(long unixTime)
        {
            this.unixTimestamp = unixTime;
        }

        public DateTime ToDateTime()
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimestamp);
        }
    }
}