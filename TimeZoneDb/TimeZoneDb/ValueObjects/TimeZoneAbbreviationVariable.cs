using System;

namespace TimeZoneDb.ValueObjects
{
    /// <summary>
    ///     represents a value for the variable in a <see cref="TimeZoneAbbreviationFormat" />
    /// </summary>
    public struct TimeZoneAbbreviationVariable
    {
        #region Ctors

        public TimeZoneAbbreviationVariable(String value)
            : this()
        {
            Value = value;
        }

        #endregion

        #region Properties

        public String Value { get; private set; }

        #endregion
    }
}