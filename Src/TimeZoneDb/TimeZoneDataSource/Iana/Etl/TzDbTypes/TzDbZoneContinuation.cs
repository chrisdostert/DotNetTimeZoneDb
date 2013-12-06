using System;
using TimeZoneDb.Entities;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public struct TzDbZoneContinuation
    {
        #region Ctors

        public TzDbZoneContinuation(TimeSpan gmtOff, String rules, TimeZoneAbbreviationFormat format,
            TzDbAbstractDateTime until = null)
            : this()
        {
            Gmtoff = gmtOff;
            Rules = rules;
            Format = format;
            Until = until;
        }

        #endregion

        #region Properties

        public TimeSpan Gmtoff { get; private set; }
        public String Rules { get; private set; }
        public TimeZoneAbbreviationFormat Format { get; private set; }
        public TzDbAbstractDateTime Until { get; private set; }

        #endregion

        #region Conversion Methods

        public TimeZoneImplementation ToTimeZoneImplementation(
            AbstractDaylightSavingsAdjustment daylightSavingsAdjustment)
        {
            var effectiveTo = new Moment();

            if (null != Until)
            {
                // TODO: Add proper DST offset.. 
                effectiveTo = Until.ToMoment(Gmtoff, TimeSpan.Zero);
            } 
                // if until is null set this to max moment
            else
            {
                effectiveTo = new Moment(long.MaxValue);
            }

            return new TimeZoneImplementation(effectiveTo, Gmtoff, daylightSavingsAdjustment, Format);
        }

        #endregion
    }
}