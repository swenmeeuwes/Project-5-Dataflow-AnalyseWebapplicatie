using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class UnixTimestamp
    {
        public long unixTimestamp { get; }

        public UnixTimestamp(long unixTimestamp)
        {
            if (unixTimestamp < 0)
                throw new ArgumentOutOfRangeException();

            this.unixTimestamp = unixTimestamp;
        }

        public DateTime ToDateTime()
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimestamp).ToLocalTime();
        }
    }
}