using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class ZoneExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out TzDbZone? tzDbZone)
        {
            // the value of the first token of every link entry is Link
            const String linkRegex = "Zone";
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, linkRegex))
            {
                try
                {
                    // dequeue Zone token and discard
                    unProcessedTokens.Dequeue();

                    string ianaId = unProcessedTokens.Dequeue().Value;
                    TimeSpan gmtOff = TimeSpanExpression.Parse(unProcessedTokens);
                    string rules = unProcessedTokens.Dequeue().Value;
                    TimeZoneAbbreviationFormat format = TimeZoneAbbreviationFormatExpression.Parse(unProcessedTokens);
                    // until is optional
                    TzDbAbstractDateTime until;
                    TzDbDateTimeExpression.TryParse(unProcessedTokens, out until);

                    tzDbZone = new TzDbZone(gmtOff, rules, format, ianaId, until);

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
                    throw new Exception("Error occured while parsing zone expression", e);
                }
            }

            tzDbZone = null;
            return false;
        }

        #endregion
    }
}