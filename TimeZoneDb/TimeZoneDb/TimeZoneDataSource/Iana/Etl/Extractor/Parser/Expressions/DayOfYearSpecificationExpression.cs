using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class DayOfYearSpecificationExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out DayOfYearSpecification? dayOfYearSpecification)
        {
            MonthOfYear? monthOfYear;
            if (MonthOfYearExpression.TryParse(unProcessedTokens, out monthOfYear))
            {
                AbstractDayOfMonthSpecification dayOfMonthSpecification;
                if (!DayOfMonthSpecificationExpression.TryParse(unProcessedTokens, out dayOfMonthSpecification))
                {
                    // default is first day of the month
                    dayOfMonthSpecification = new ExplicitDayOfMonthSpecification(1);
                }

                dayOfYearSpecification = new DayOfYearSpecification(monthOfYear.Value, dayOfMonthSpecification);
                return true;
            }

            dayOfYearSpecification = null;
            return false;
        }

        #endregion
    }
}