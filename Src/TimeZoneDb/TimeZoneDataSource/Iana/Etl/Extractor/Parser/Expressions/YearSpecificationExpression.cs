using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class YearSpecificationExpression
    {
        #region Methods

        public static AbstractYearSpecification Parse(Queue<Token> unProcessedTokens)
        {
            AbstractYearSpecification yearSpecification;
            if (!TryParse(unProcessedTokens, out yearSpecification))
            {
                string msg = String.Format("Unable to parse year specification expression: {0}",
                    unProcessedTokens.Peek().Value);
                throw new Exception(msg);
            }

            return yearSpecification;
        }

        public static bool TryParse(Queue<Token> unProcessedTokens, out AbstractYearSpecification yearSpecification)
        {
            const String maximumYearRegex = @"(max)|(maximum)";
            const String minimumYearRegex = @"(min)|(minimum)";
            const String explicitYearRegex = @"\d{4}";
            string yearSpecificationExpressionRegex = String.Format("^({0}|{1}|{2})$", maximumYearRegex,
                minimumYearRegex, explicitYearRegex);

            // dont consume token unless token is matched
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, yearSpecificationExpressionRegex))
            {
                string yearSpecificationString = unProcessedTokens.Dequeue().Value;
                if (Regex.IsMatch(yearSpecificationString, maximumYearRegex))
                {
                    yearSpecification = new MaximumYearSpecification();
                    return true;
                }
                if (Regex.IsMatch(yearSpecificationString, minimumYearRegex))
                {
                    yearSpecification = new MinimumYearSpecification();
                    return true;
                }
                try
                {
                    yearSpecification = new ExplicitYearSpecification(int.Parse(yearSpecificationString));
                    return true;
                }
                catch (Exception e)
                {
                    string msg = String.Format("Unable to parse year specification expression: {0}",
                        yearSpecificationString);
                    throw new Exception(msg, e);
                }
            }
            yearSpecification = null;
            return false;
        }

        #endregion
    }
}