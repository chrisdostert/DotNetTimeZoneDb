using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public abstract class TzDbAbstractDateTime
    {
        #region Ctors

        protected TzDbAbstractDateTime(Date date, AbstractTime time)
        {
            Date = date;
            Time = time;
        }

        #endregion

        #region Properties

        public Date Date { get; private set; }
        public AbstractTime Time { get; private set; }

        #endregion

        #region Conversion Methods

        public abstract Moment ToMoment(TimeSpan standardOffset, TimeSpan daylightSavingsOffset);

        #endregion
    }
}