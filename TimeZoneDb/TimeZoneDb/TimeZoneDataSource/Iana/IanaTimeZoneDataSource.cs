using TimeZoneDb.Repositories;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Loader;

namespace TimeZoneDb.TimeZoneDataSource.Iana
{
    public class IanaTimeZoneDataSource : ITimeZoneDataSource
    {
        #region Ctors

        public IanaTimeZoneDataSource(ITzDbLoader tzDbLoader = null)
        {
            _tzDbLoader = tzDbLoader ?? new TzDbLoader();
        }

        #endregion

        #region Fields

        private readonly ITzDbLoader _tzDbLoader;

        #endregion

        #region Methods

        public void Load(ITimeZoneRepository timeZoneRepository, IDaylightSavingsAdjustmentRepository daylightSavingsAdjustmentRepository)
        {
            _tzDbLoader.Load(timeZoneRepository, daylightSavingsAdjustmentRepository);
        }

        #endregion
    }
}