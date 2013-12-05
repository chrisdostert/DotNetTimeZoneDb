using TimeZoneDb.Repositories;

namespace TimeZoneDb.TimeZoneDataSource
{
    /// <summary>
    ///     Writes data to the timezone database with the latest timezone data from
    ///     the data source
    /// </summary>
    public interface ITimeZoneDataSource
    {
        #region Methods

        void Load(ITimeZoneRepository timeZoneRepository, IDaylightSavingsAdjustmentRepository daylightSavingsAdjustmentRepository);
        
        #endregion
    }
}