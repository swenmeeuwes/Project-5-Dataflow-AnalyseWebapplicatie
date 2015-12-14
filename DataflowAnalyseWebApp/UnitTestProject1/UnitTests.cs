using System;
using NUnit.Framework;
using DataflowAnalyseWebApp.Controllers;
using DataflowAnalyseWebApp;
using DataflowAnalyseWebApp.Models;
using System.Collections.Generic;

namespace DataflowAnalyseWebAppTest
{
    [TestFixture]
    public class UnitInformationTest
    {
        private List<Position> data;

        [Test]
        public void GetData()
        {
            UnitInformation unit = new UnitInformation();
            data = unit.GetData();
 
            Assert.IsFalse(data.Count == 0);
        }
    }
}
