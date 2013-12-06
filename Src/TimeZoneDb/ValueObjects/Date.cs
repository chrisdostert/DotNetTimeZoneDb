using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.ValueObjects
{
    public class Date
    {
        #region Ctors

        public Date(int year, MonthOfYear monthOfYear, int dayOfMonth)
        {
            Year = year;
            MonthOfYear = monthOfYear;
            DayOfMonth = dayOfMonth;
        }

        public Date(int year, DayOfYearSpecification dayOfYearSpecification)
        {
            Year = year;
            MonthOfYear = dayOfYearSpecification.MonthOfYear;
            DayOfMonth = dayOfYearSpecification.DayOfMonthSpecification.ToDayOfMonth(year, MonthOfYear);
        }

        #endregion

        #region Properties

        public int Year { get; private set; }
        public MonthOfYear MonthOfYear { get; private set; }
        public int DayOfMonth { get; private set; }

        #endregion
    }
}