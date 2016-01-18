using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp;
using System.Collections.Generic;
using DataflowAnalyseWebApp.Models;
using DataflowAnalyseWebApp.Controllers;
using DataflowAnalyseWebApp.Models.PositionModels;

/// <summary>
/// Unit test class for the analysis class UnitInformation.cs
/// 
/// </summary>
namespace UnitInformationTest {
    [TestClass]
    public class UnitInformationTest {

        private UnitInformationController unit;
        private IEnumerable<PositionBadUnit> data;

        [TestInitialize]
        public void SetUp() {
            unit = new UnitInformationController();
            data = unit.Get();
        }

        [TestMethod]
        public void TestGetData() {
            int length = 0;
            foreach (PositionBadUnit p in data)
                length++;
            Assert.IsTrue(length == 0);
        }

        //[TestMethod]
        //public void TestFrequenceBadConnections() {
        //    Dictionary<long, int> occurences = unit.CheckOccurence();
        //    List<long> frequentBadConnections = unit.GetFrequenceBadConnections();
        //    bool hundredOrMore = true;
        //    foreach (long unit in frequentBadConnections) {
        //        if (!occurences.ContainsKey(unit)) {
        //            hundredOrMore = false;
        //        }
        //    }
        //    Assert.IsTrue(hundredOrMore);
        //}

        [TestCleanup]
        public void TearDown() {
            unit = null;
            data = null;
        }
    }
}
