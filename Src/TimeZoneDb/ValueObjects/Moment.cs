using System;

namespace TimeZoneDb.ValueObjects
{
    /// <summary>
    ///     Represents a single unambiguous instant in time as seconds from the unix epoch
    /// </summary>
    public struct Moment : IComparable<Moment>
    {
        #region Ctors

        public Moment(long secondsFromUnixEpoch)
            : this()
        {
            SecondsFromUnixEpoch = secondsFromUnixEpoch;
        }

        #endregion

        #region Fields

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region Properties

        private long SecondsFromUnixEpoch { get; set; }

        #endregion

        #region Utility Methods

        private static long GetCurrentUnixTimestampInSeconds()
        {
            return (long) (DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        private static long GetUnixTimeStampInSeconds(DateTime dateTime)
        {
            return (long) (dateTime - UnixEpoch).TotalSeconds;
        }

        #endregion

        #region Creation Methods

        /// <summary>
        ///     Creates a <see cref="Moment" /> representing now
        /// </summary>
        /// <returns></returns>
        public static Moment Create()
        {
            return new Moment(GetCurrentUnixTimestampInSeconds());
        }

        /// <summary>
        ///     Creates a <see cref="Moment" /> representing the <paramref name="utcDateTime" /> given
        ///     Note: <paramref name="utcDateTime" /> is assumed to be in UTC
        /// </summary>
        /// <returns></returns>
        public static Moment Create(DateTime utcDateTime)
        {
            return new Moment(GetUnixTimeStampInSeconds(utcDateTime));
        }

        #endregion

        #region Utility Methods

        public int CompareTo(Moment other)
        {
            long difference = SecondsFromUnixEpoch - other.SecondsFromUnixEpoch;
            if (0 == difference)
            {
                return 0;
            }
            if (0 > difference)
            {
                return -1;
            }
            return 1;
        }

        #endregion

        #region Operators

        public static bool operator <(Moment instant1, Moment instant2)
        {
            return instant1.SecondsFromUnixEpoch < instant2.SecondsFromUnixEpoch;
        }

        public static bool operator >(Moment instant1, Moment instant2)
        {
            return instant1.SecondsFromUnixEpoch > instant2.SecondsFromUnixEpoch;
        }

        public static Moment operator +(Moment instant1, Moment instant2)
        {
            return new Moment(instant1.SecondsFromUnixEpoch + instant2.SecondsFromUnixEpoch);
        }

        public static Moment operator -(Moment instant1, Moment instant2)
        {
            return new Moment(instant1.SecondsFromUnixEpoch - instant2.SecondsFromUnixEpoch);
        }

        #endregion

        #region Conversion Methods

        /// <summary>
        ///     Returns a UTC <see cref="DateTime" /> for the current <see cref="Moment" />
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return UnixEpoch.AddSeconds(SecondsFromUnixEpoch);
        }

        #endregion
    }
}