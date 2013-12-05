using System;
using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class TzDbDateExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out Date date)
        {
            try
            {
                AbstractYearSpecification yearSpecification;
                if (YearSpecificationExpression.TryParse(unProcessedTokens, out yearSpecification))
                {
                    DayOfYearSpecification? dayOfYearSpecification;
                    if (!DayOfYearSpecificationExpression.TryParse(unProcessedTokens, out dayOfYearSpecification))
                    {
                        // default is first day of first month
                        dayOfYearSpecification = new DayOfYearSpecification(MonthOfYear.January,
                            new ExplicitDayOfMonthSpecification(1));
                    }
                    date = new Date(yearSpecification.ToYear(), dayOfYearSpecification.Value);
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to parse Date specification expression", e);
            }

            date = null;
            return false;
        }

        #endregion
    }
}