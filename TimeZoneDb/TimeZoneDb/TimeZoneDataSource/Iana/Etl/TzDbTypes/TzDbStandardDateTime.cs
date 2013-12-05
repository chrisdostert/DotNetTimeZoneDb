using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public class TzDbStandardDateTime : TzDbAbstractDateTime
    {
        #region Ctors

        public TzDbStandardDateTime(Date date, StandardTime time)
            : base(date, time)
        {
        }

        #endregion

        #region Conversion Methods

        public override Moment ToMoment(TimeSpan standardOffset, TimeSpan daylightSavingsOffset)
        {
            // convert to UTC
            var dateTime = new DateTime(Date.Year, (int) Date.MonthOfYear, Date.DayOfMonth, Time.Hours, Time.Minutes,
                Time.Seconds, DateTimeKind.Utc);
            dateTime = dateTime.AddHours(standardOffset.Hours);
            dateTime = dateTime.AddMinutes(standardOffset.Minutes);
            dateTime = dateTime.AddSeconds(standardOffset.Seconds);

            return Moment.Create(dateTime);
        }

        #endregion
    }
}