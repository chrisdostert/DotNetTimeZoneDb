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

        private static Lazy<ITimeZoneDbUseCases> _timeZoneDbUseCases = new Lazy<ITimeZoneDbUseCases>(() => new TimeZoneDbUseCases());

        #endregion

        [TestMethod]
        public void GetAllTimeZones()
        {
            var allTimeZones = _timeZoneDbUseCases.Value.GetAllTimeZones();
            // explicit expected count is arbitrary but should be a reasonable number
            Assert.IsTrue(allTimeZones.Count() > 400);
        }

        [TestMethod]
        public void GetTimeZoneWithIanaId()
        {
            var timeZone = _timeZoneDbUseCases.Value.GetTimeZoneWithIanaId("America/Los_Angeles");
            Assert.IsNotNull(timeZone);
        }

        [TestMethod]
        public void GetTimeZoneWithIanaId_WrongCaseResolves()
        {
            var timeZone = _timeZoneDbUseCases.Value.GetTimeZoneWithIanaId("america/los_angeles");
            Assert.IsNotNull(timeZone);
        }
    }
}
