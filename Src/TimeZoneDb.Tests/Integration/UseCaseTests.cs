using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeZoneDb.UseCases;

namespace TimeZoneDb.Tests.Integration
{
    [TestClass]
    public class UseCaseTests
    {
        #region Fields

        private static ITimeZoneDbUseCases _timeZoneDbUseCases;

        #endregion

        [TestInitialize]
        public void Initialize()
        {
            // this became static because instantiating this thing takes ages
            if (_timeZoneDbUseCases == null)
            {
                _timeZoneDbUseCases = new TimeZoneDbUseCases();
            }
        }

        [TestMethod]
        public void GetAllTimeZones()
        {
            var allTimeZones = _timeZoneDbUseCases.GetAllTimeZones();
            // explicit expected count is arbitrary but should be a reasonable number
            Assert.IsTrue(allTimeZones.Count() > 400);
        }

        [TestMethod]
        public void GetTimeZoneWithIanaId()
        {
            var timeZone = _timeZoneDbUseCases.GetTimeZoneWithIanaId("America/Los_Angeles");
            Assert.IsNotNull(timeZone);
        }

        [TestMethod]
        public void GetTimeZoneWithIanaId_WrongCaseResolves()
        {
            var timeZone = _timeZoneDbUseCases.GetTimeZoneWithIanaId("america/los_angeles");
            Assert.IsNotNull(timeZone);
        }
    }
}
