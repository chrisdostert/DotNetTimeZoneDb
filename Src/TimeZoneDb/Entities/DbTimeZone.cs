using System;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.Entities
{
    /// <summary>
    /// Represents a Time Zone and its related information
    /// </summary>
    public class DbTimeZone
    {
        #region Ctors

        public DbTimeZone(String ianaId, GeographyPoint coordinates, String iso31661Alpha2,
            IEnumerable<TimeZoneAlias> aliases,
            IEnumerable<TimeZoneImplementation> implementations, Guid? id = null)
        {
            IanaId = ianaId;
            Coordinates = coordinates;
            Iso31661Alpha2 = iso31661Alpha2;
            Aliases = aliases;
            Implementations = implementations.OrderBy(implementation => implementation.EffectiveTo);
            Id = id ?? Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }

        public String Name
        {
            get
            {
                // if the MicrosoftId for this DbTimeZone is known use it as the name;
                // otherwise use the IanaId
                if (MicrosoftId != null)
                {
                    return MicrosoftId;
                }

                return IanaId;
            }
        }

        /// <summary>
        ///     The identifier the IANA uses to uniquely identify this <see cref="DbTimeZone" />
        /// </summary>
        public String IanaId { get; private set; }

        /// <summary>
        ///     The identifier Microsoft uses to uniquely identify this <see cref="DbTimeZone" />
        /// </summary>
        public String MicrosoftId { get; private set; }

        public GeographyPoint Coordinates { get; private set; }
        public String Iso31661Alpha2 { get; private set; }
        public IEnumerable<TimeZoneAlias> Aliases { get; private set; }

        /// <summary>
        ///     Ordered Implementations
        /// </summary>
        public IEnumerable<TimeZoneImplementation> Implementations { get; private set; }

        #endregion

        #region Methods

        public TimeSpan GetDstOffset(Moment moment)
        {
            TimeZoneImplementation effectiveImplementation = GetEffectiveImplementation(moment);
            TimeSpan rawOffset = GetRawOffset(moment);

            // Guard: if the effectiveImplementation has no Daylight Savings Adjustment
            if (null == effectiveImplementation.DaylightSavingsAdjustment)
            {
                return TimeSpan.Zero;
            }

            AbstractDaylightSavingsAdjustment daylightSavingsAdjustment =
                effectiveImplementation.DaylightSavingsAdjustment;
            TimeSpan dstOffset = daylightSavingsAdjustment.GetAdjustmentToStandardOffset(rawOffset, moment);
            return dstOffset;
        }

        public TimeSpan GetRawOffset(Moment moment)
        {
            return GetEffectiveImplementation(moment).StandardUtcOffset;
        }

        public String GetAbbreviation(Moment moment)
        {
            TimeZoneImplementation effectiveImplementation = GetEffectiveImplementation(moment);
            return effectiveImplementation.GetTimeZoneAbbreviation(moment);
        }

        public void UpdateMicrosoftId(String microsoftId)
        {
            MicrosoftId = microsoftId;
        }

        #endregion

        #region Utility Methods

        private TimeZoneImplementation GetEffectiveImplementation(Moment moment)
        {
            // since implementations are ordered we can just take the first with
            // an EffectiveTo larger than moment
            return Implementations.First(implementation => implementation.EffectiveTo > moment);
        }

        #endregion
    }
}