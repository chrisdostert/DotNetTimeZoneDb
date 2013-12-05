using System;

namespace TimeZoneDb.ValueObjects
{
    public abstract class AbstractTime
    {
        #region Ctors

        internal AbstractTime(TimeSpan timeSpan)
            : this(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds)
        {
        }

        internal AbstractTime(int hours = 0, int minutes = 0, int seconds = 0,
            int milliseconds = 0)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }

        #endregion

        #region Properties

        public int Hours { get; private set; }

        public int Minutes { get; private set; }

        public int Seconds { get; private set; }

        public int Milliseconds { get; private set; }

        #endregion
    }
}