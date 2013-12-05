namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer
{
    public interface ITzDbTransformer
    {
        ITransformerDatabaseReader Transform();
    }
}