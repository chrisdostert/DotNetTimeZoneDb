using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer
{
    public class InMemoryTransformerDatabase : ITransformerDatabase
    {
        #region Ctors

        public InMemoryTransformerDatabase()
        {
            DropAndReCreate();
        }

        #endregion

        #region Properties

        public List<AbstractDaylightSavingsAdjustment> DaylightSavingsAdjustments { get; private set; }

        public List<DbTimeZone> TimeZones { get; private set; }

        #endregion

        #region Methods

        public void DropAndReCreate()
        {
            DaylightSavingsAdjustments = new List<AbstractDaylightSavingsAdjustment>();
            TimeZones = new List<DbTimeZone>();
        }

        #endregion
    }
}