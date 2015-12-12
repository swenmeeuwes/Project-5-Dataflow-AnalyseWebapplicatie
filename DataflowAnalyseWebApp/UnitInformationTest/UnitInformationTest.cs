using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp;
using DataflowAnalyseWebApp.Models;
using System.Collections.Generic;

namespace UnitInformationTest {
    [TestClass]
    public class UnitInformationTest {

        private UnitInformation unit;
        private List<Position> allData;
        private List<Position> filteredData;

        [TestInitialize]
        public void SetUp() {
            unit = new UnitInformation();
            allData = unit.GetData();
            filteredData = unit.GetBadConnections();
        }

        [TestMethod]
        public void TestGetallData() {
            Assert.AreNotEqual(allData.Count, 0);
        }

        [TestMethod]
        public void TestCheckOccurance() {
            Dictionary<long, int> occurances = unit.CheckOccurence();
            Assert.AreNotEqual(occurances.Count, 0);
        }

        [TestMethod]
        public void TestGetBadConnections() {
            Assert.IsTrue(filteredData.Count < allData.Count);
        }

        [TestMethod]
        public void TestGetFrequenceBadConnections() {
            Dictionary<long, int> occurences = unit.CheckOccurence();
            List<long> frequentBadConnection = unit.GetFrequenceBadConnections();
            bool moreThan100 = true;
            foreach(long unitId in frequentBadConnection) {
                if (!occurences.ContainsKey(unitId)) {
                    moreThan100 = false;
                }
            }
            Assert.IsTrue(moreThan100);
        }

        [TestCleanup]
        public void TearDown() {
            unit = null;
        }
    }
}
