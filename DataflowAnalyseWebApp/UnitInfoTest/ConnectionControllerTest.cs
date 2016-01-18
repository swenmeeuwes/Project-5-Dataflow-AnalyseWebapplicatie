using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataflowAnalyseWebApp.Controllers;
using DataflowAnalyseWebApp.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitInfoTest
{
    [TestClass]
    public class ConnectionControllerTest
    {
        private ConnectionController connectionController;
        private List<Connection> dummyList;
        

        [TestInitialize]
        public void setUp()
        {
            connectionController = new ConnectionController();
            dummyList = new List<Connection>();

            Connection connection1 = new Connection();
            connection1.connectionSpeed = 5;
            connection1.unitId = 1;
            Connection connection2 = new Connection();
            connection2.connectionSpeed = 6;
            connection2.unitId = 1;
            Connection connection3 = new Connection();
            connection3.connectionSpeed = 7;
            connection3.unitId = 1;

            dummyList.Add(connection1);
            dummyList.Add(connection2);
            dummyList.Add(connection3);

        }


        [TestMethod]
        public void TestAverage()
        {
           List<Connection> filteredDummyList = connectionController.calculateAverage(dummyList);
           Assert.AreEqual(filteredDummyList[0].connectionSpeed, 6.00);


        }
    }
}
