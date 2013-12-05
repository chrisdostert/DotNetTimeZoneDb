using TimeZoneDb.Repositories;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Loader
{
    public interface ITzDbLoader
    {
        void Load(ITimeZoneRepository timeZoneRepository,
            IDaylightSavingsAdjustmentRepository daylightSavingsAdjustmentRepository);
    }
}