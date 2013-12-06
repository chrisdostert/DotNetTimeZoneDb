using System;
using System.Collections.Generic;
using System.Spatial;
using System.Text;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class ZoneDescriptionExpression
    {
        public static bool TryParse(Queue<Token> unProcessedTokens, out TzDbZoneDescription zoneDescription)
        {
            // the value of the first token of every zone description entry is an iso 3166-2 character code
            const String zoneDescriptionRegex = "^[A-Z]{2}$";
            Token token = unProcessedTokens.Peek();
            if (token.TokenType == TokenType.Stringliteral && Regex.IsMatch(token.Value, zoneDescriptionRegex))
            {
                string countryCode = unProcessedTokens.Dequeue().Value;
                GeographyPoint coordinates = GeographyPointExpression.Parse(unProcessedTokens);

                string tz = unProcessedTokens.Dequeue().Value;
                String comments;
                if (!TryParseComments(unProcessedTokens, out comments))
                {
                    throw new Exception("Unable to parse zone description comments expression");
                }

                zoneDescription = new TzDbZoneDescription(countryCode, coordinates, tz, comments);

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
            zoneDescription = null;
            return false;
        }

        private static bool TryParseComments(Queue<Token> unProcessedTokens, out String comments)
        {
            // zone description comments are optional but if specified appear at end of line and can contain
            // spaces thus just keep adding all tokens until we hit the end of the line
            var commentsStringBuilder = new StringBuilder();
            while (unProcessedTokens.Peek().TokenType != TokenType.EndLine)
            {
                commentsStringBuilder.Append(unProcessedTokens.Dequeue().Value + " ");
            }
            comments = commentsStringBuilder.ToString();
            return true;
        }
    }
}