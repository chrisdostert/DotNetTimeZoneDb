using System;

namespace TimeZoneDb.ValueObjects
{
    /// <summary>
    ///     Represents "standard" time in a given <see cref="TimeZone" /> on a given <see cref="Date" />
    /// </summary>
    public class StandardTime : AbstractTime
    {
        #region Ctors

        public StandardTime(TimeSpan timeSpan) : base(timeSpan)
        {
        }

        public StandardTime(int hours = 0, int minutes = 0, int seconds = 0,
            int milliseconds = 0)
            : base(hours, minutes, seconds, milliseconds)
        {
        }

        #endregion
    }
}