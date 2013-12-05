using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer
{
    public interface ITransformerDatabase
    {
        #region Properties

        List<AbstractDaylightSavingsAdjustment> DaylightSavingsAdjustments { get; }
        List<DbTimeZone> TimeZones { get; }

        #endregion

        #region Methods

        // drops and recreates all tables (deletes everything and starts with a clean slate)
        void DropAndReCreate();

        #endregion
    }
}