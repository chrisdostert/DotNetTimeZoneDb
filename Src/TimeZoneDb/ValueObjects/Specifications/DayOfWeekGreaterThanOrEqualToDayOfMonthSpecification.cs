using System;

namespace TimeZoneDb.ValueObjects.Specifications
{
    public class DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification : AbstractDayOfMonthSpecification
    {
        #region Ctors

        /// <summary>
        /// </summary>
        /// <param name="dayOfWeek">Specifies the day of the week to recur on</param>
        /// <param name="dayOfMonth">
        ///     Specifies the day of the month to recur on. Negative numbers are wrapped i.e. they represent counting backwards
        ///     from
        ///     the end of the month. Example: -1 would represent the last day of the month
        /// </param>
        public DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(DayOfWeek dayOfWeek, int dayOfMonth)
        {
            DayOfWeek = dayOfWeek;
            DayOfMonth = dayOfMonth;
        }

        #endregion

        #region Properties

        public DayOfWeek DayOfWeek { get; private set; }
        public int DayOfMonth { get; private set; }

        #endregion

        #region Methods

        public override int ToDayOfYear(int year, MonthOfYear month)
        {
            int dayOfMonth = ToDayOfMonth(year, month);
            return new DateTime(year, (int) month, dayOfMonth).DayOfYear;
        }

        public override int ToDayOfMonth(int year, MonthOfYear month)
        {
            // walk backwards from the last day of the month until we hit the desired DayOfWeek
            int daysFromMinDayOfMonth = 0;
            while (daysFromMinDayOfMonth < 7)
            {
                int currentDayOfMonth = DayOfMonth + daysFromMinDayOfMonth++;
                DayOfWeek currentDayOfWeek = new DateTime(year, (int) month, currentDayOfMonth).DayOfWeek;
                if (currentDayOfWeek == DayOfWeek)
                {
                    return currentDayOfMonth;
                }
            }
            string msg = String.Format("Error finding {0} >= {1} in {2} {3}", DayOfWeek, DayOfMonth, month, year);
            throw new Exception(msg);
        }

        #endregion
    }
}