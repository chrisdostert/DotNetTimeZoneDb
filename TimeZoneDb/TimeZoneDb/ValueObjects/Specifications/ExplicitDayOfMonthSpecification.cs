using System;

namespace TimeZoneDb.ValueObjects.Specifications
{
    public class ExplicitDayOfMonthSpecification : AbstractDayOfMonthSpecification
    {
        #region Ctors

        public ExplicitDayOfMonthSpecification(int dayOfMonth)
        {
            DayOfMonth = dayOfMonth;
        }

        #endregion

        #region Properties

        public int DayOfMonth { get; private set; }

        #endregion

        #region Conversion Methods

        public override int ToDayOfYear(int year, MonthOfYear month)
        {
            int dayOfMonth = ToDayOfMonth(year, month);
            return new DateTime(year, (int) month, dayOfMonth).DayOfYear;
        }

        public override int ToDayOfMonth(int year, MonthOfYear month)
        {
            return DayOfMonth;
        }

        #endregion
    }
}