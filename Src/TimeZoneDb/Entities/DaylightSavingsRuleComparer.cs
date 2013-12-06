using System;
using System.Collections.Generic;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.Entities
{
    public class DaylightSavingsRuleComparer : IComparer<DaylightSavingsRule>
    {
        #region Ctors

        /// <summary>
        ///     Compares <see cref="DaylightSavingsRule" />s by the day on which they occur.
        ///     Note: This does not take into account the time at which they occur
        /// </summary>
        /// <param name="year">the year to use for comparisons to be made</param>
        public DaylightSavingsRuleComparer(int year)
        {
            _year = year;
        }

        #endregion

        #region Fields

        private readonly int _year;

        #endregion

        #region Methods

        public int Compare(DaylightSavingsRule x, DaylightSavingsRule y)
        {
            // build DateTime
            DateTime xDateTime = GetDateTime(x);
            DateTime yDateTime = GetDateTime(y);
            return xDateTime.CompareTo(yDateTime);
        }

        private DateTime GetDateTime(DaylightSavingsRule daylightSavingsRule)
        {
            MonthOfYear xMonth = daylightSavingsRule.OccursIn;
            int xDay = daylightSavingsRule.OccursOnSpecification.ToDayOfMonth(_year, xMonth);
            return new DateTime(_year, (int) xMonth, xDay);
        }

        #endregion
    }
}