using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor
{
    public interface ITzDbExtractor
    {
        #region Methods

        IExtractorDatabaseReader Extract();

        #endregion
    }
}