using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp.Controllers;
using System.Collections.Generic;
using DataflowAnalyseWebApp.Models;

namespace UnitInfoTest
{
    [TestClass]
    public class IgnitionTest
    {
        private IgnitionController ignition;
        private List<Event> dummyData;
        Dictionary<long, int> dummyDictionary;

        [TestInitialize]
        public void SetUp()
        {
            ignition = new IgnitionController();

            dummyData = new List<Event>();
            Event event1 = new Event();
            event1.unitId = 54451;
            Event event2 = new Event();
            event2.unitId = 12345;

            dummyData.Add(event1);
            dummyData.Add(event2);
            dummyData.Add(event2);

            dummyDictionary = new Dictionary<long, int>();
            dummyDictionary.Add(1234, 5);
            dummyDictionary.Add(4355, 21);
            dummyDictionary.Add(5454, 6);
            dummyDictionary.Add(3812, 68);

        }

        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<long, int> test = ignition.RemoveDuplicatesFromList(dummyData);
            Assert.IsTrue(test.Count == 2);
        }

        [TestMethod]
        public void TestAverage()
        {
            double testDouble = ignition.CalculateAverage(dummyDictionary);

            Assert.IsTrue(testDouble == (5+21+6+68) / 4);
        }
    }
}
