using System;
using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class TimeSpanExpression
    {
        #region Methods

        public static TimeSpan Parse(Queue<Token> unProcessedTokens)
        {
            try
            {
                string timeSpanExpressionString = unProcessedTokens.Dequeue().Value;
                return TimeSpan.Parse(timeSpanExpressionString);
            }
            catch (Exception e)
            {
                string msg = String.Format("Unable to parse {0}", typeof (TimeSpanExpression).Name);

                throw new Exception(msg, e);
            }
        }

        #endregion
    }
}