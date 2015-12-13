using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp;
using System.Collections.Generic;
using DataflowAnalyseWebApp.Models;


/// <summary>
/// Unit test class for the analysis class UnitInformation.cs
/// 
/// </summary>
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
        public void TestGetData() {
            Assert.IsFalse(allData.Count == 0);
        }

        [TestMethod]
        public void TestGetBadConnections() {
            Assert.IsTrue(allData.Count > filteredData.Count);
        }

        [TestMethod]
        public void TestCheckOccurance() {
            Dictionary<long, int> occurences = unit.CheckOccurence();
            Assert.IsFalse(occurences.Count == 0);
        }

        [TestMethod]
        public void TestFrequenceBadConnections() {
            Dictionary<long, int> occurences = unit.CheckOccurence();
            List<long> frequentBadConnections = unit.GetFrequenceBadConnections();
            bool hundredOrMore = true;
            foreach (long unit in frequentBadConnections) {
                if (!occurences.ContainsKey(unit)) {
                    hundredOrMore = false;
                }
            }
            Assert.IsTrue(hundredOrMore);
        }

        [TestCleanup]
        public void TearDown() {
            unit = null;
            allData = null;
            filteredData = null;
        }
    }
}
