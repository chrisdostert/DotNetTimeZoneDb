namespace TimeZoneDb.ValueObjects.Specifications
{
    public class ExplicitYearSpecification : AbstractYearSpecification
    {
        #region Ctors

        public ExplicitYearSpecification(int year)
        {
            Year = year;
        }

        #endregion

        #region Properties

        public int Year { get; private set; }

        #endregion

        #region Methods

        public override int ToYear()
        {
            return Year;
        }

        #endregion
    }
}