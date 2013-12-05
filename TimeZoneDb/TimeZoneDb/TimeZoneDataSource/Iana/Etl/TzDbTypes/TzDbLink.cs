using System;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public struct TzDbLink
    {
        #region Ctors

        public TzDbLink(String linkFrom, String linkTo)
            : this()
        {
            LinkFrom = linkFrom;
            LinkTo = linkTo;
        }

        #endregion

        #region Properties

        public String LinkFrom { get; private set; }
        public String LinkTo { get; private set; }

        #endregion

        #region ConversionMethods

        public TimeZoneAlias ToTimeZoneAlias()
        {
            return new TimeZoneAlias(LinkTo);
        }

        #endregion
    }
}