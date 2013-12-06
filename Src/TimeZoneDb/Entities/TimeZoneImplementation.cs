using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.Entities
{
    /// <summary>
    ///     A specification for generating a time zones implementation
    /// </summary>
    public class TimeZoneImplementation
    {
        #region Ctors

        public TimeZoneImplementation(
            Moment effectiveTo,
            TimeSpan standardUtcOffset,
            AbstractDaylightSavingsAdjustment daylightSavingsAdjustment,
            TimeZoneAbbreviationFormat timeZoneAbbreviationFormat,
            Guid? id = null)
        {
            EffectiveTo = effectiveTo;
            StandardUtcOffset = standardUtcOffset;
            DaylightSavingsAdjustment = daylightSavingsAdjustment;
            TimeZoneAbbreviationFormat = timeZoneAbbreviationFormat;
            Id = id ?? Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }

        /// <summary>
        ///     the <see cref="Moment" /> this <see cref="TimeZoneImplementation" /> goes out of effect
        /// </summary>
        public Moment EffectiveTo { get; private set; }

        public TimeSpan StandardUtcOffset { get; private set; }
        public AbstractDaylightSavingsAdjustment DaylightSavingsAdjustment { get; private set; }

        /// <summary>
        ///     a specification for generating the abbreviation for this time zone
        /// </summary>
        public TimeZoneAbbreviationFormat TimeZoneAbbreviationFormat { get; private set; }

        #endregion

        #region Methods

        public String GetTimeZoneAbbreviation(Moment moment)
        {
            if (null != DaylightSavingsAdjustment)
            {
                return DaylightSavingsAdjustment.GetTimeZoneAbbreviation(StandardUtcOffset, TimeZoneAbbreviationFormat,
                    moment);
            }

            return TimeZoneAbbreviationFormat.Format;
        }

        #endregion
    }
}