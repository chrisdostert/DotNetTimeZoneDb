using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeZoneDb.TimeZoneDataSource;
using TimeZoneDb.TimeZoneDataSource.Iana;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Loader;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer;
using TimeZoneDb.UseCases;

namespace TimeZoneDb.Tests.Integration
{
    [TestClass]
    public class UseCaseTests
    {
        #region Fields

        private ITimeZoneDbUseCases _timeZoneDbUseCases = new TimeZoneDbUseCases();

        #endregion

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
            var timeZone = _timeZoneDbUseCases.GetTimeZoneWithIanaId("America/New_York");
            Assert.IsNotNull(timeZone);
        }
    }
}
