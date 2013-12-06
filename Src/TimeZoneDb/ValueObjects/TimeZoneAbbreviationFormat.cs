using System;

namespace TimeZoneDb.ValueObjects
{
    public struct TimeZoneAbbreviationFormat
    {
        #region Ctors

        public TimeZoneAbbreviationFormat(String format)
            : this()
        {
            Format = format;
        }

        #endregion

        #region Properties

        public String Format { get; private set; }

        #endregion

        #region Methods

        public String GetAbbreviation(TimeZoneAbbreviationVariable abbreviationVariable)
        {
            return String.Format(Format, abbreviationVariable);
        }

        #endregion
    }
}