using System;
using System.Collections.Generic;
using TimeZoneDb.Entities;
using TimeZoneDb.TimeZoneDataSource;

namespace TimeZoneDb.UseCases
{
    public interface ITimeZoneDbUseCases
    {
        #region Methods

        IEnumerable<DbTimeZone> GetAllTimeZones();
        DbTimeZone GetTimeZoneWithIanaId(String ianaId);
        void Refresh(List<ITimeZoneDataSource> timeZoneDataSources = null);

        #endregion
    }
}