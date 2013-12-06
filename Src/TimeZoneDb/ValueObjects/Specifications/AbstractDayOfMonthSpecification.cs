namespace TimeZoneDb.ValueObjects.Specifications
{
    /// <summary>
    ///     A set of values which, given a year, specify the date(s) something will occur
    /// </summary>
    public abstract class AbstractDayOfMonthSpecification
    {
        public abstract int ToDayOfYear(int year, MonthOfYear month);
        public abstract int ToDayOfMonth(int year, MonthOfYear month);
    }
}