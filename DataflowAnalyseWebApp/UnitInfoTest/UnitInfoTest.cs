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
        private List<PositionBadUnit> data;
        private int randomNumber;
        private Random r;

        [TestInitialize]
        public void SetUp() {
            unit = new UnitInformationController();
            data = new List<PositionBadUnit>();
            data.Add(new PositionBadUnit(145554484, 3, 1, 45));
            data.Add(new PositionBadUnit(100002100, 4, 4, 100));
            data.Add(new PositionBadUnit(888877755, 10, 8, 56));
            data.Add(new PositionBadUnit(222020120, 20, 7, 99));
            data.Add(new PositionBadUnit(100021444, 2, 16, 8992));
            r = new Random();
        }

        [TestMethod]
        public void TestAverageCorrect() {
            int average = unit.FindAverage(data);
            Assert.AreEqual((45 + 100 + 56 + 99 + 8992) / 5, average);
        }

        [TestMethod]
        public void TestAverageInCorrect() {
            int average = unit.FindAverage(data);
            randomNumber = r.Next(0, 1800);
            Assert.AreNotEqual(randomNumber, average);
        }

        [TestMethod]
        public void TestMaxCorrect() {
            int max = unit.FindMaximum(data);
            Assert.AreEqual(8992, max);
        }

        [TestMethod]
        public void TestMaxIncorrect() {
            int max = unit.FindMaximum(data);
            randomNumber = r.Next(0, 8900);
            Assert.AreNotEqual(randomNumber, max);
        }

        [TestMethod]
        public void TestMinCorrect() {
            int min = unit.FindMinimum(data);
            Assert.IsTrue(min == 45);
        }

        [TestMethod]
        public void TestMinIncorrect() {
            int min = unit.FindMinimum(data);
            randomNumber = r.Next(46, 9999);
            Assert.IsFalse(min == randomNumber);
        }

        [TestMethod]
        public void TestSumCorrect() {
            int sum = unit.FindSum(data);
            Assert.AreEqual(45 + 100 + 56 + 99 + 8992, sum);
        }

        [TestMethod]
        public void TestSumIncorrect() {
            int sum = unit.FindSum(data);
            randomNumber = r.Next(0, 9291);
            Assert.AreNotEqual(randomNumber, sum);
        }


        [TestCleanup]
        public void TearDown() {
            unit = null;
            data = null;
            r = null;
        }
    }
}
