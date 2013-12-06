using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.Entities
{
    public abstract class AbstractDaylightSavingsAdjustment
    {
        #region Ctors

        protected AbstractDaylightSavingsAdjustment(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the offset at the <paramref name="moment" /> requested
        /// </summary>
        /// <param name="standardOffset"></param>
        /// <param name="moment"></param>
        /// <returns></returns>
        public abstract TimeSpan GetAdjustmentToStandardOffset(TimeSpan standardOffset, Moment moment);

        public abstract String GetTimeZoneAbbreviation(TimeSpan standardOffset, TimeZoneAbbreviationFormat format,
            Moment moment);

        #endregion
    }
}