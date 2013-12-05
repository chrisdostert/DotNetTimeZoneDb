using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser
{
    public interface ITzDbParser
    {
        void Parse(IFileInfo fileInfo, IExtractorDatabase extractorDatabase);
    }
}