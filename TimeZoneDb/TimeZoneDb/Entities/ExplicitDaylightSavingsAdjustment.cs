using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.Entities
{
    public class ExplicitDaylightSavingsAdjustment : AbstractDaylightSavingsAdjustment
    {
        #region Ctors

        public ExplicitDaylightSavingsAdjustment(TimeSpan adjustmentToStandardOffset, Guid? id = null)
            : base(id)
        {
            AdjustmentToStandardOffset = adjustmentToStandardOffset;
        }

        #endregion

        #region Fields

        public TimeSpan AdjustmentToStandardOffset { get; private set; }

        #endregion

        #region Methods

        public override TimeSpan GetAdjustmentToStandardOffset(TimeSpan standardOffset, Moment moment)
        {
            return AdjustmentToStandardOffset;
        }

        public override string GetTimeZoneAbbreviation(TimeSpan standardOffset, TimeZoneAbbreviationFormat format,
            Moment moment)
        {
            return String.Format(format.Format);
        }

        #endregion
    }
}