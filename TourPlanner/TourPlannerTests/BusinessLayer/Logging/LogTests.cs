using Microsoft.VisualStudio.TestTools.UnitTesting;
using TourPlanner.BusinessLayer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.Logging.Tests
{
    [TestClass()]
    public class LogTests
    {
        [TestMethod()]
        public void LogFatalTest()
        {
            Assert.Fail();
        }
    }
}