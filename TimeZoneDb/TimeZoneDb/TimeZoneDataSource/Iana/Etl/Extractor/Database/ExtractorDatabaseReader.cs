using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database
{
    public class ExtractorDatabaseReader : IExtractorDatabaseReader
    {
        #region Ctors

        public ExtractorDatabaseReader(IExtractorDatabase extractorDatabase)
        {
            _extractorDatabase = extractorDatabase;
        }

        #endregion

        #region Fields

        private readonly IExtractorDatabase _extractorDatabase;

        #endregion

        #region Properties

        public IEnumerable<TzDbLink> TzDbLinks
        {
            get { return _extractorDatabase.Links.AsEnumerable(); }
        }

        public IEnumerable<TzDbRule> TzDbRules
        {
            get { return _extractorDatabase.Rules.AsEnumerable(); }
        }

        public IEnumerable<TzDbZoneDescription> TzDbZoneDescriptions
        {
            get { return _extractorDatabase.ZoneDescriptions.AsEnumerable(); }
        }

        public IEnumerable<TzDbZoneDefinition> TzDbZoneDefinitions
        {
            get { return _extractorDatabase.ZoneDefinitions.AsEnumerable(); }
        }

        #endregion
    }
}