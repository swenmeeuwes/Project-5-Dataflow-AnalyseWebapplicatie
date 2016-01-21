using System;
using DataflowAnalyseWebApp.Models.MonitoringModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitInfoTest {
    [TestClass]
    public class DiskSpaceTest {

        private List<MonitoringDiskSpace> data;

        [TestInitialize]
        public void SetUp() {
            data = new List<MonitoringDiskSpace>();
            data.Add(new MonitoringDiskSpace(1, new DateTime(), 10, "Empty"));
            data.Add(new MonitoringDiskSpace(1, new DateTime(), 72, "Half full"));
            data.Add(new MonitoringDiskSpace(1, new DateTime(), 98, "Full"));
            data.Add(new MonitoringDiskSpace(1, new DateTime(), 84, "Almost full"));
        }

        [TestMethod]
        public void TestDiskSpace() {
            bool correct = true;
            foreach (MonitoringDiskSpace m in data) {
                if (m.percentUsed <= 25) {
                    if (!m.diskSpaceStatus.Equals("Empty")) {
                        correct = false;
                    }
                }
                else if (m.percentUsed > 25 && m.percentUsed < 75) {
                    if (!m.diskSpaceStatus.Equals("Half full")) {
                        correct = false;
                    }
                }
                else if (m.percentUsed >= 75 && m.percentUsed < 90) {
                    if (!m.diskSpaceStatus.Equals("Almost full")) {
                        correct = false;
                    }
                }
                else if (m.percentUsed >= 90) {
                    if (!m.diskSpaceStatus.Equals("Full")) {
                        correct = false;
                    }
                }
            }
            Assert.IsTrue(correct);
        }
    }
}
