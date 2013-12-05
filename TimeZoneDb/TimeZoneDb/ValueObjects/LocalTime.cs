using System;

namespace TimeZoneDb.ValueObjects
{
    /// <summary>
    ///     Represents "wall" time in a given <see cref="TimeZone" /> on a given <see cref="Date" />
    /// </summary>
    public class LocalTime : AbstractTime
    {
        #region Ctors

        public LocalTime(TimeSpan timeSpan) : base(timeSpan)
        {
        }

        public LocalTime(int hours = 0, int minutes = 0, int seconds = 0,
            int milliseconds = 0)
            : base(hours, minutes, seconds, milliseconds)
        {
        }

        #endregion
    }
}