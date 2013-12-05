using System;
using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class ZoneContinuationExpression
    {
        #region Methods

        public static TzDbZoneContinuation Parse(Queue<Token> unProcessedTokens, String timeZoneName)
        {
            try
            {
                TimeSpan gmtOff = TimeSpanExpression.Parse(unProcessedTokens);
                string rules = unProcessedTokens.Dequeue().Value;
                TimeZoneAbbreviationFormat format =
                    TimeZoneAbbreviationFormatExpression.Parse(unProcessedTokens);
                // until is optional
                TzDbAbstractDateTime until;
                TzDbDateTimeExpression.TryParse(unProcessedTokens, out until);

                Token endLineToken = unProcessedTokens.Dequeue();
                // guard end of line token was present
                if (endLineToken.TokenType != TokenType.EndLine)
                {
                    string msg = String.Format(
                        "received unexpected token type:{0} when token type should have been {1}",
                        endLineToken.TokenType, TokenType.EndLine);
                    throw new Exception(msg);
                }

                return new TzDbZoneContinuation(gmtOff, rules, format, until);
            }
            catch (Exception e)
            {
                throw new Exception("Error occured while parsing zone continuation expression", e);
            }
        }

        #endregion
    }
}