namespace TimeZoneDb.ValueObjects.Specifications
{
    /// <summary>
    ///     Specifies the minimum possible year
    /// </summary>
    public class MinimumYearSpecification : AbstractYearSpecification
    {
        #region Methods

        public override int ToYear()
        {
            return int.MinValue;
        }

        #endregion
    }
}