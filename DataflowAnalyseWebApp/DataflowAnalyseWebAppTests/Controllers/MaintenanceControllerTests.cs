using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace DataflowAnalyseWebApp.Controllers.Tests
{
    [TestClass()]
    public class MaintenanceControllerTests
    {
        Random random = new Random();
        [TestMethod()]
        public void GetDistance()
        {
            GeoCoordinate position1 = new GeoCoordinate(random.Next(180) - 90, random.Next(360) - 180);
            GeoCoordinate position2 = new GeoCoordinate(random.Next(180) - 90, random.Next(360) - 180);
            Assert.AreNotEqual(Double.NaN, position1.GetDistanceTo(position2));
        }
    }
}