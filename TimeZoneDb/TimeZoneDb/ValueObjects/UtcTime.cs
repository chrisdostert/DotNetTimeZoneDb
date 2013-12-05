using System;

namespace TimeZoneDb.ValueObjects
{
    public class UtcTime : AbstractTime
    {
        #region Ctors

        public UtcTime(TimeSpan timeSpan) : base(timeSpan)
        {
        }

        public UtcTime(int hours = 0, int minutes = 0, int seconds = 0,
            int milliseconds = 0)
            : base(hours, minutes, seconds, milliseconds)
        {
        }

        #endregion
    }
}