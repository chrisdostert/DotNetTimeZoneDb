using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database
{
    public interface IExtractorDatabaseReader
    {
        #region Properties

        IEnumerable<TzDbLink> TzDbLinks { get; }
        IEnumerable<TzDbRule> TzDbRules { get; }
        IEnumerable<TzDbZoneDescription> TzDbZoneDescriptions { get; }
        IEnumerable<TzDbZoneDefinition> TzDbZoneDefinitions { get; }

        #endregion
    }
}