using System;

namespace TimeZoneDb.ValueObjects.Specifications
{
    /// <summary>
    ///     Specifies the day of month should be based on the last occurrence of a particular day of week
    ///     in that month
    /// </summary>
    public class LastWeekdayOfMonthSpecification : AbstractDayOfMonthSpecification
    {
        #region Ctors

        public LastWeekdayOfMonthSpecification(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        #endregion

        #region Properties

        public DayOfWeek DayOfWeek { get; private set; }

        #endregion

        #region Methods

        public override int ToDayOfYear(int year, MonthOfYear month)
        {
            int dayOfMonth = ToDayOfMonth(year, month);
            return new DateTime(year, (int) month, dayOfMonth).DayOfYear;
        }

        public override int ToDayOfMonth(int year, MonthOfYear month)
        {
            int daysInMonth = DateTime.DaysInMonth(year, (int) month);

            // walk backwards from the last day of the month until we hit the desired DayOfWeek
            int daysFromLastDayOfMonth = 0;
            while (daysFromLastDayOfMonth < 7)
            {
                int currentDayOfMonth = daysInMonth - daysFromLastDayOfMonth++;
                DayOfWeek currentDayOfWeek = new DateTime(year, (int) month, currentDayOfMonth).DayOfWeek;
                if (currentDayOfWeek == DayOfWeek)
                {
                    return currentDayOfMonth;
                }
            }
            string msg = "Error finding the last occurrence of a weekday in a month";
            throw new Exception(msg);
        }

        #endregion
    }
}