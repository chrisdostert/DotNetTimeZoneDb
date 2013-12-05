using System;
using TimeZoneDb.ValueObjects;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.Entities
{
    /// <summary>
    ///     A specification for generating AdjustmentToStandardOffset adjustments
    /// </summary>
    public class DaylightSavingsRule
    {
        #region Ctors

        public DaylightSavingsRule(
            TimeSpan adjustmentToStandardOffset,
            AbstractYearSpecification utcEffectiveFromSpecification,
            AbstractYearSpecification utcEffectiveToSpecification,
            MonthOfYear occursIn,
            AbstractDayOfMonthSpecification occursOnSpecification,
            AbstractTime occursAt,
            TimeZoneAbbreviationVariable timeZoneAbbreviationVariable,
            Guid? id = null
            )
        {
            AdjustmentToStandardOffset = adjustmentToStandardOffset;
            UtcEffectiveFromSpecification = utcEffectiveFromSpecification;
            UtcEffectiveToSpecification = utcEffectiveToSpecification;
            OccursIn = occursIn;
            OccursOnSpecification = occursOnSpecification;
            OccursAt = occursAt;
            TimeZoneAbbreviationVariable = timeZoneAbbreviationVariable;
            Id = id ?? Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }
        public TimeSpan AdjustmentToStandardOffset { get; private set; }
        public AbstractYearSpecification UtcEffectiveFromSpecification { get; private set; }
        public AbstractYearSpecification UtcEffectiveToSpecification { get; private set; }
        public MonthOfYear OccursIn { get; private set; }
        public AbstractDayOfMonthSpecification OccursOnSpecification { get; private set; }
        public AbstractTime OccursAt { get; private set; }
        public TimeZoneAbbreviationVariable TimeZoneAbbreviationVariable { get; private set; }

        #endregion

        #region Conversion Methods

        /// <summary>
        ///     Processes this <see cref="DaylightSavingsRule" /> for a given <paramref name="standardOffset" />,
        ///     <paramref name="daylightSavingsAdjustment" />,
        ///     and <paramref name="year" />
        /// </summary>
        /// <param name="standardOffset"></param>
        /// <param name="daylightSavingsAdjustment"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Moment ToMoment(TimeSpan standardOffset, TimeSpan daylightSavingsAdjustment, int year)
        {
            MonthOfYear monthRuleOccurs = OccursIn;
            int dayRuleOccurs = OccursOnSpecification.ToDayOfMonth(year, monthRuleOccurs);

            // handle rules specified in UtcTime
            var utcTime = OccursAt as UtcTime;
            if (null != utcTime)
            {
                var dateTime = new DateTime(year, (int) monthRuleOccurs, dayRuleOccurs, OccursAt.Hours, OccursAt.Minutes,
                    OccursAt.Seconds, DateTimeKind.Utc);
                return Moment.Create(dateTime);
            }

            // handle rules specified in StandardTime
            var standardTime = OccursAt as StandardTime;
            if (null != standardTime)
            {
                // convert to UTC
                var dateTime = new DateTime(year, (int) monthRuleOccurs, dayRuleOccurs, OccursAt.Hours, OccursAt.Minutes,
                    OccursAt.Seconds, DateTimeKind.Utc);
                dateTime = dateTime.AddHours(standardOffset.Hours);
                dateTime = dateTime.AddMinutes(standardOffset.Minutes);
                dateTime = dateTime.AddSeconds(standardOffset.Seconds);

                return Moment.Create(dateTime);
            }

            // handle rules specified in LocalTime
            var localTime = OccursAt as LocalTime;
            if (null != localTime)
            {
                // convert to UTC
                var dateTime = new DateTime(year, (int) monthRuleOccurs, dayRuleOccurs, OccursAt.Hours, OccursAt.Minutes,
                    OccursAt.Seconds, DateTimeKind.Utc);
                dateTime = dateTime.AddHours(standardOffset.Hours + daylightSavingsAdjustment.Hours);
                dateTime = dateTime.AddMinutes(standardOffset.Minutes + daylightSavingsAdjustment.Minutes);
                dateTime = dateTime.AddSeconds(standardOffset.Seconds + daylightSavingsAdjustment.Seconds);

                return Moment.Create(dateTime);
            }

            string msg = String.Format("An exception occured while converting {0} to a {1}",
                typeof (DaylightSavingsRule),
                typeof (Moment));
            throw new Exception(msg);
        }

        #endregion
    }
}