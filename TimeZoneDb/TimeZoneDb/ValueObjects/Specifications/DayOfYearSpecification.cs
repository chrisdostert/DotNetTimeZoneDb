namespace TimeZoneDb.ValueObjects.Specifications
{
    public struct DayOfYearSpecification
    {
        #region Ctors

        public DayOfYearSpecification(MonthOfYear monthOfYear, AbstractDayOfMonthSpecification dayOfMonthSpecification)
            : this()
        {
            MonthOfYear = monthOfYear;
            DayOfMonthSpecification = dayOfMonthSpecification;
        }

        #endregion

        #region Properties

        public MonthOfYear MonthOfYear { get; private set; }
        public AbstractDayOfMonthSpecification DayOfMonthSpecification { get; private set; }

        #endregion

        #region Conversion Methods

        /// <summary>
        ///     returns the explicit day of year this <see cref="DayOfYearSpecification" />
        ///     specifies for a given year
        /// </summary>
        /// <param name="year">the year for which the day of year should be calculated</param>
        /// <returns></returns>
        public int ToDayOfYear(int year)
        {
            return DayOfMonthSpecification.ToDayOfYear(year, MonthOfYear);
        }

        #endregion
    }
}