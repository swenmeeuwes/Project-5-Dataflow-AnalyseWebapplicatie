using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using DataflowAnalyseWebApp.Models;

namespace DataflowAnalyseWebApp.Controllers.Tests
{
    [TestClass()]
    public class MaintenanceControllerTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Arguments out of range are inappropriately allowed.")]
        public void GetDistance()
        {
            // Boundary values to test (Boundary value test)
            int[] latitudeBoundaryValues = new int[] { -91, -90, -89, -1, 0, 1, 89, 90, 91 };
            int[] longitudeBoundaryValues = new int[] { -181, -180, -179, -1, 0, 1, 179, 180, 181 };

            // The test shouldn't run if the array's aren't the same length, otherwise it might ignore some tests.
            Assert.AreEqual(latitudeBoundaryValues.Length, longitudeBoundaryValues.Length);

            for (int i = 0; i < latitudeBoundaryValues.Length; i++)
            {
                GeoCoordinate position1 = new GeoCoordinate(latitudeBoundaryValues[i], longitudeBoundaryValues[i]);
                GeoCoordinate position2 = new GeoCoordinate(latitudeBoundaryValues[i], longitudeBoundaryValues[i]);
                Assert.AreNotEqual(Double.NaN, position1.GetDistanceTo(position2));
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Arguments out of range are inappropriately allowed.")]
        public void ToDateTime()
        {
            UnixTimestamp[] unixTimestamp = new UnixTimestamp[] { new UnixTimestamp(-1), new UnixTimestamp(0), new UnixTimestamp(1), new UnixTimestamp(1452009478) };

            for (int i = 0; i < unixTimestamp.Length; i++)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(unixTimestamp[i].unixTimestamp).ToLocalTime();

                Assert.AreEqual(dateTime, unixTimestamp[i].ToDateTime());
            }
        }
    }
}