namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer
{
    public enum TokenType
    {
        UnAssigned,
        BeginFile,
        EndFile,
        BeginLine,
        EndLine,
        Stringliteral,
        IntegerLiteral
    }
}