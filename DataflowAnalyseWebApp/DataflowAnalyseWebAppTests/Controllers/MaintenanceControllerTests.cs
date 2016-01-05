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
        public void GetDistance()
        {
            // Boundary values to test (Boundary value test)
            int[] latitudeBoundaryValues = new int[] { -90, 0, 90 };
            int[] longitudeBoundaryValues = new int[] { -180, 0, 180 };

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
        public void ToDateTime()
        {
            // (GMT): Tue, 05 Jan 2016 15:57:58 GMT
            UnixTimestamp unixTimestamp = new UnixTimestamp(1452009478);
            DateTime dateTime = new DateTime(2016, 1, 5, 15, 57, 58, DateTimeKind.Utc);
            Assert.AreEqual(dateTime, unixTimestamp.ToDateTime());
        }
    }
}