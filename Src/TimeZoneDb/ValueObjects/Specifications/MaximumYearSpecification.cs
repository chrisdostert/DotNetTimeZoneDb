namespace TimeZoneDb.ValueObjects.Specifications
{
    /// <summary>
    ///     Specifies the maximum possible year
    /// </summary>
    public class MaximumYearSpecification : AbstractYearSpecification
    {
        #region Methods

        public override int ToYear()
        {
            return int.MaxValue;
        }

        #endregion
    }
}