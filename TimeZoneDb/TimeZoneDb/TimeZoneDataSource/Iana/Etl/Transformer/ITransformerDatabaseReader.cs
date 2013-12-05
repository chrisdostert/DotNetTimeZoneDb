using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer
{
    public interface ITransformerDatabaseReader
    {
        #region Properties

        IEnumerable<AbstractDaylightSavingsAdjustment> OffsetAdjustments { get; }
        IEnumerable<DbTimeZone> TimeZones { get; }

        #endregion
    }
}