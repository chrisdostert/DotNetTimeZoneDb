using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class TimeZoneAbbreviationFormatExpression
    {
        #region Methods

        public static TimeZoneAbbreviationFormat Parse(Queue<Token> unProcessedTokens)
        {
            try
            {
                string timeZoneAbbreviationFormatString = unProcessedTokens.Dequeue().Value;

                // make compatible with String.Format
                timeZoneAbbreviationFormatString = Regex.Replace(timeZoneAbbreviationFormatString, "%s", "{0}");
                return new TimeZoneAbbreviationFormat(timeZoneAbbreviationFormatString);
            }
            catch (Exception e)
            {
                string msg = String.Format("Unable to parse {0}", typeof (TimeZoneAbbreviationFormatExpression).Name);
                throw new Exception(msg, e);
            }
        }

        #endregion
    }
}