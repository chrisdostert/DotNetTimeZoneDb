using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class LinkExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out TzDbLink? tzDbLink)
        {
            // the value of the first token of every link entry is Link
            const String linkRegex = "Link";
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, linkRegex))
            {
                try
                {
                    // dequeue link token and discard
                    unProcessedTokens.Dequeue();

                    String linkFrom = unProcessedTokens.Dequeue().Value;
                    String linkTo = unProcessedTokens.Dequeue().Value;

                    tzDbLink = new TzDbLink(linkFrom, linkTo);

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
                    throw new Exception("Error occured while parsing link expression", e);
                }
            }
            tzDbLink = null;
            return false;
        }

        #endregion
    }
}