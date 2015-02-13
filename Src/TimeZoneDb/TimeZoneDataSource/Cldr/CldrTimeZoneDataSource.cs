using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneDb.Entities;
using TimeZoneDb.Repositories;
using TimeZoneMapper;
using TimeZoneMapper.TZMappers;

namespace TimeZoneDb.TimeZoneDataSource.Cldr
{
    public class CldrTimeZoneDataSource : ITimeZoneDataSource
    {
        #region Ctors

        public CldrTimeZoneDataSource()
        {
            _tzMapper = TimeZoneMap.OnlineWithFallbackValuesTZMapper;
        }

        #endregion

        #region Fields

        private readonly ITZMapper _tzMapper; 

        #endregion

        #region Methods

        public void Load(ITimeZoneRepository timeZoneRepository, IDaylightSavingsAdjustmentRepository daylightSavingsAdjustmentRepository)
        {
            string[] supportedIanaIds = _tzMapper.GetAvailableTZIDs();
            foreach (DbTimeZone timeZone in timeZoneRepository.GetAll())
            {
                if (supportedIanaIds.Any(ianaId => ianaId == timeZone.IanaId))
                {
                    TimeZoneInfo timeZoneInfo = _tzMapper.MapTZID(timeZone.IanaId);
                    string microsoftId = timeZoneInfo.Id;
                    timeZone.UpdateMicrosoftId(microsoftId);
                    timeZoneRepository.Update(timeZone);
                }
            }
        }

        #endregion
    }
}
