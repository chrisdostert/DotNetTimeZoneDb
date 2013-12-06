using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer
{
    public interface ITzDbTokenizer
    {
        Queue<Token> Tokenize(IFileInfo fileInfo);
    }
}