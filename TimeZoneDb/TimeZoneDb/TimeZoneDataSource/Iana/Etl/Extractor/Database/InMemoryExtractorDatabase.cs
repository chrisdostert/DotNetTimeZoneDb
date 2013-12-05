using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database
{
    public class InMemoryExtractorDatabase : IExtractorDatabase
    {
        #region Ctors

        public InMemoryExtractorDatabase()
        {
            DropAndReCreate();
        }

        #endregion

        #region Properties

        public List<TzDbLink> Links { get; private set; }

        public List<TzDbRule> Rules { get; private set; }

        public List<TzDbZoneDescription> ZoneDescriptions { get; private set; }

        public List<TzDbZoneDefinition> ZoneDefinitions { get; private set; }

        #endregion

        #region Methods

        public void DropAndReCreate()
        {
            Links = new List<TzDbLink>();
            Rules = new List<TzDbRule>();
            ZoneDescriptions = new List<TzDbZoneDescription>();
            ZoneDefinitions = new List<TzDbZoneDefinition>();
        }

        #endregion
    }
}