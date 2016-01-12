using System;
using NUnit.Framework;
using DataflowAnalyseWebApp;
using System.Collections.Generic;
using DataflowAnalyseWebApp.Models;

namespace UnitInformationTest {
    [TestFixture]
    public class UnitInformationTest {

        private UnitInformation unit;
        private List<Position> allData;
        private List<Position> filteredData;

        [SetUp]
        public void SetUp() {
            unit = new UnitInformation();
            allData = unit.GetData();
            filteredData = unit.GetBadConnections();
        }

        [Test]
        public void TestGetData() {
            Assert.IsFalse(allData.Count == 0);
        }

        [Test]
        public void TestGetBadConnections() {
            Assert.IsTrue(allData.Count > filteredData.Count);
        }

        [Test]
        public void TestCheckOccurance() {
            Dictionary<long, int> occurences = unit.CheckOccurence();
            Assert.IsFalse(occurences.Count == 0);
        }

        [Test]
        public void testFrequenceBadConnections() {
            Dictionary<long, int> occurences = unit.CheckOccurence();
            List<long> frequentBadConnections = unit.GetFrequenceBadConnections();
            bool hundredOrMore = true;
            foreach(long unit in frequentBadConnections) {
                if (!occurences.ContainsKey(unit)) {
                    hundredOrMore = false;
                }
            }
            Assert.IsTrue(hundredOrMore);
        }

        [TearDown]
        public void TearDown() {
            unit = null;
            allData = null;
            filteredData = null;
        }
    }
}
