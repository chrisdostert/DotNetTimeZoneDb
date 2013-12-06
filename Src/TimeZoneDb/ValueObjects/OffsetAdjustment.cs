using System;

namespace TimeZoneDb.ValueObjects
{
    public class OffsetAdjustment
    {
        #region Ctors

        public OffsetAdjustment(TimeSpan adjustment, int utcEffectiveFrom, int utcEffectiveTo, Moment occurs,
            String timeZoneAbbreviation)
        {
            Adjustment = adjustment;
            UtcEffectiveFrom = utcEffectiveFrom;
            UtcEffectiveTo = utcEffectiveTo;
            Occurs = occurs;
            TimeZoneAbbreviation = timeZoneAbbreviation;
        }

        #endregion

        #region Properties

        public TimeSpan Adjustment { get; set; }
        public int UtcEffectiveFrom { get; set; }
        public int UtcEffectiveTo { get; set; }
        public Moment Occurs { get; set; }
        public String TimeZoneAbbreviation { get; set; }

        #endregion
    }
}