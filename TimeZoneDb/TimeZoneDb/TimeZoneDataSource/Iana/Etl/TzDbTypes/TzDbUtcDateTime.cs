using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public class TzDbUtcDateTime : TzDbAbstractDateTime
    {
        #region Ctors

        public TzDbUtcDateTime(Date date, UtcTime time)
            : base(date, time)
        {
        }

        #endregion

        #region Conversion Methods

        public override Moment ToMoment(TimeSpan standardOffset, TimeSpan daylightSavingsOffset)
        {
            var dateTime = new DateTime(Date.Year, (int) Date.MonthOfYear, Date.DayOfMonth, Time.Hours, Time.Minutes,
                Time.Seconds, DateTimeKind.Utc);

            return Moment.Create(dateTime);
        }

        #endregion
    }
}