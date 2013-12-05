using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.Repositories;
using TimeZoneDb.TimeZoneDataSource;
using TimeZoneDb.TimeZoneDataSource.Cldr;
using TimeZoneDb.TimeZoneDataSource.Iana;

namespace TimeZoneDb.UseCases
{
    public class TimeZoneDbUseCases : ITimeZoneDbUseCases
    {
        #region Ctors

        /// <summary>
        /// Initializes the Use Cases supported by the TimeZoneDb
        /// </summary>
        /// <param name="daylightSavingsAdjustmentRepository"></param>
        /// <param name="timeZoneDataSources">
        /// Allows overriding the default data sources with <paramref name="timeZoneDataSources"/>.
        /// Note: If your sources are dependent on each other or have preference over each other, 
        /// make sure the order in which they appear in <paramref name="timeZoneDataSources"/> 
        /// is the order in which you want them to be loaded i.e. source at index 0 will load 
        /// prior to source at index 1
        /// </param>
        /// <param name="timeZoneRepository"></param>
        public TimeZoneDbUseCases(ITimeZoneRepository timeZoneRepository = null, IDaylightSavingsAdjustmentRepository daylightSavingsAdjustmentRepository = null, List<ITimeZoneDataSource> timeZoneDataSources = null)
        {
            _timeZoneRepository = timeZoneRepository ?? new InMemoryTimeZoneRepository();
            _daylightSavingsAdjustmentRepository = daylightSavingsAdjustmentRepository ?? new InMemoryDaylightSavingsAdjustmentRepository();
            Load(timeZoneDataSources);
        }

        #endregion

        #region Fields

        private readonly ITimeZoneRepository _timeZoneRepository;
        private readonly IDaylightSavingsAdjustmentRepository _daylightSavingsAdjustmentRepository;

        #endregion

        #region Properties
        public static List<ITimeZoneDataSource> DefaultTimeZoneDataSources
        {
            get { return new List<ITimeZoneDataSource>() { new IanaTimeZoneDataSource(), new CldrTimeZoneDataSource() }; }
        }

        #endregion

        #region Methods

        public void Refresh(List<ITimeZoneDataSource> timeZoneDataSources = null)
        {
            Load(timeZoneDataSources);
        }

        public IEnumerable<Entities.DbTimeZone> GetAllTimeZones()
        {
            return _timeZoneRepository.GetAll();
        }

        public Entities.DbTimeZone GetTimeZoneWithIanaId(string ianaId)
        {
            return _timeZoneRepository.GetByIanaId(ianaId);
        }

        #endregion

        #region Utility Methods

        private void Load(List<ITimeZoneDataSource> timeZoneDataSources = null)
        {
            if (null == timeZoneDataSources || !timeZoneDataSources.Any())
            {
                timeZoneDataSources = DefaultTimeZoneDataSources;
            }

            foreach (var timeZoneDataSource in timeZoneDataSources)
            {
                timeZoneDataSource.Load(_timeZoneRepository, _daylightSavingsAdjustmentRepository);
            }
        }

        #endregion
    }
}