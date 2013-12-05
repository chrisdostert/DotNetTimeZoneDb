using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class TimeZoneAbbreviationVariableExpression
    {
        #region Methods

        public static TimeZoneAbbreviationVariable Parse(Queue<Token> unProcessedTokens)
        {
            try
            {
                const String nullRegex = "^-$";
                string timeZoneAbbreviationVariableString = unProcessedTokens.Dequeue().Value;

                // Guard: handle null values;
                if (Regex.IsMatch(timeZoneAbbreviationVariableString, nullRegex))
                {
                    timeZoneAbbreviationVariableString = null;
                }
                return new TimeZoneAbbreviationVariable(timeZoneAbbreviationVariableString);
            }
            catch (Exception e)
            {
                string msg = String.Format("Unable to parse {0}", typeof (TimeZoneAbbreviationVariableExpression).Name);
                throw new Exception(msg, e);
            }
        }

        #endregion
    }
}