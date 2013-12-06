using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.Entities;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer
{
    public class TransformerDatabaseReader : ITransformerDatabaseReader
    {
        #region Ctors

        public TransformerDatabaseReader(ITransformerDatabase transformerDatabase)
        {
            _transformerDatabase = transformerDatabase;
        }

        #endregion

        #region Fields

        private readonly ITransformerDatabase _transformerDatabase;

        #endregion

        #region Properties

        public IEnumerable<AbstractDaylightSavingsAdjustment> OffsetAdjustments
        {
            get { return _transformerDatabase.DaylightSavingsAdjustments.AsEnumerable(); }
        }

        public IEnumerable<DbTimeZone> TimeZones
        {
            get { return _transformerDatabase.TimeZones.AsEnumerable(); }
        }

        #endregion
    }
}