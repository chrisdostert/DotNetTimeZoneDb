using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;
using TimeZoneDb.ValueObjects;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class RuleExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out TzDbRule? tzDbRule)
        {
            // the value of the first token of every link entry is Rule
            const String ruleRegex = "Rule";
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, ruleRegex))
            {
                try
                {
                    // dequeue Rule token and discard
                    unProcessedTokens.Dequeue();

                    string name = unProcessedTokens.Dequeue().Value;
                    AbstractYearSpecification from = YearSpecificationExpression.Parse(unProcessedTokens);
                    AbstractYearSpecification to = ParseToExpression(unProcessedTokens, from);
                    string type = unProcessedTokens.Dequeue().Value;
                    MonthOfYear @in = MonthOfYearExpression.Parse(unProcessedTokens);
                    AbstractDayOfMonthSpecification on = DayOfMonthSpecificationExpression.Parse(unProcessedTokens);
                    AbstractTime at = TzDbTimeExpression.Parse(unProcessedTokens);
                    TimeSpan save = TimeSpanExpression.Parse(unProcessedTokens);
                    TimeZoneAbbreviationVariable letter = TimeZoneAbbreviationVariableExpression.Parse(unProcessedTokens);

                    tzDbRule = new TzDbRule(name, from, to, type, @in, on, at, save, letter);

                    Token endLineToken = unProcessedTokens.Dequeue();
                    // guard end of line token was present
                    if (endLineToken.TokenType != TokenType.EndLine)
                    {
                        string msg = String.Format(
                            "received unexpected token type:{0} when token type should have been {1}",
                            endLineToken.TokenType, TokenType.EndLine);
                        throw new Exception(msg);
                    }

                    return true;
                }
                catch (Exception e)
                {
                    string msg = String.Format("Unable to parse {0}", typeof (RuleExpression).Name);

                    throw new Exception(msg, e);
                }
            }
            tzDbRule = null;
            return false;
        }

        private static AbstractYearSpecification ParseToExpression(Queue<Token> unProcessedTokens,
            AbstractYearSpecification from)
        {
            const String onlyRegex = "^only$";

            if (Regex.IsMatch(unProcessedTokens.Peek().Value, onlyRegex))
            {
                // dequeue only token and discard
                unProcessedTokens.Dequeue();

                return from;
            }

            return YearSpecificationExpression.Parse(unProcessedTokens);
        }

        #endregion
    }
}