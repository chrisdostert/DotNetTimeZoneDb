using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database
{
    public interface IExtractorDatabase
    {
        #region Properties

        List<TzDbLink> Links { get; }
        List<TzDbRule> Rules { get; }
        List<TzDbZoneDescription> ZoneDescriptions { get; }
        List<TzDbZoneDefinition> ZoneDefinitions { get; }

        #endregion

        #region Methods

        // drops and recreates all tables (deletes everything and starts with a clean slate)
        void DropAndReCreate();

        #endregion
    }
}