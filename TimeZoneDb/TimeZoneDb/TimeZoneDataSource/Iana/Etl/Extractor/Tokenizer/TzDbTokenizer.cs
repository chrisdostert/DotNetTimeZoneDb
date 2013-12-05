using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer
{
    public class TzDbTokenizer : ITzDbTokenizer
    {
        #region Constants

        private const String IntegerRegexPattern = @"^[+-]?\d+$";
        private const String StringRegexPattern = @"^(?!(^\d+$))\S+$";
        private const String TokenSeparatorRegexPattern = @"\s+";

        #endregion

        #region Methods

        public Queue<Token> Tokenize(IFileInfo fileInfo)
        {
            var tokens = new Queue<Token>();
            // add a begin file token
            tokens.Enqueue(new Token(TokenType.BeginFile, fileInfo.Name));

            LineOfFile? line = null;
            StreamReader streamReader = fileInfo.StreamReader;

            // tokenize the stream one line at a time
            while (TryReadNextLine(line, streamReader, out line))
            {
                if (!TryTokenizeLine(line.Value, tokens))
                {
                    string msg = String.Format("Unable to tokenize line: {0}", line);
                    throw new Exception(msg);
                }
            }

            // add an end file token
            tokens.Enqueue(new Token(TokenType.EndFile, fileInfo.Name));

            return tokens;
        }

        /// <summary>
        ///     Retrieves the next applicable/valid line of the stream if one exists
        /// </summary>
        /// <param name="currentLineOfFile">The current line of the file</param>
        /// <param name="streamReader">A <see cref="StreamReader" /> for the file</param>
        /// <param name="nextLineOfFile">The next non blank/comment line of the file</param>
        /// <returns></returns>
        private bool TryReadNextLine(LineOfFile? currentLineOfFile, StreamReader streamReader,
            out LineOfFile? nextLineOfFile)
        {
            // set current line number
            int currentLineNumber = 0;
            if (null != currentLineOfFile)
            {
                currentLineNumber = currentLineOfFile.Value.LineNumber;
            }

            int nextLineNumber = currentLineNumber + 1;
            while (!streamReader.EndOfStream)
            {
                string nextLineText = RemoveCommentsAndTrailingWhiteSpace(streamReader.ReadLine());

                // when we find the next non empty line return it!
                if (!String.IsNullOrWhiteSpace(nextLineText))
                {
                    nextLineOfFile = new LineOfFile(nextLineNumber, nextLineText);
                    return true;
                }

                // increment next line number
                nextLineNumber++;
            }

            nextLineOfFile = null;
            return false;
        }

        /// <summary>
        ///     Tries to tokenize the line and adds its tokens to the <paramref name="tokens" />
        /// </summary>
        /// <param name="line"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private bool TryTokenizeLine(LineOfFile line, Queue<Token> tokens)
        {
            List<string> tokenValues = Regex.Split(line.LineText, TokenSeparatorRegexPattern).ToList();

            tokens.Enqueue(new Token(TokenType.BeginLine, String.Format("Line number: {0}", line.LineNumber)));

            foreach (string tokenValue in tokenValues)
            {
                // Guard: ignore blank tokenValues
                if (String.IsNullOrWhiteSpace(tokenValue))
                {
                    continue;
                }

                Token? token;
                if (TryGetStringLiteralToken(tokenValue, out token))
                {
                    tokens.Enqueue(token.Value);
                    continue;
                }
                if (TryGetIntegerLiteralToken(tokenValue, out token))
                {
                    tokens.Enqueue(token.Value);
                    continue;
                }

                return false;
            }
            tokens.Enqueue(new Token(TokenType.EndLine, String.Format("Line number: {0}", line.LineNumber)));
            return true;
        }

        private bool TryGetStringLiteralToken(String tokenValue, out Token? stringLiteralToken)
        {
            if (Regex.IsMatch(tokenValue, StringRegexPattern))
            {
                stringLiteralToken = new Token(TokenType.Stringliteral, tokenValue);
                return true;
            }

            stringLiteralToken = null;
            return false;
        }

        private bool TryGetIntegerLiteralToken(String tokenValue, out Token? integerLiteralToken)
        {
            if (Regex.IsMatch(tokenValue, IntegerRegexPattern))
            {
                integerLiteralToken = new Token(TokenType.IntegerLiteral, tokenValue);
                return true;
            }

            integerLiteralToken = null;
            return false;
        }

        private string RemoveCommentsAndTrailingWhiteSpace(String text)
        {
            // remove comments
            int index = text.IndexOf("#", StringComparison.Ordinal);
            if (index == 0)
            {
                text = String.Empty;
            }
            else if (index > 0)
            {
                text = text.Substring(0, index - 1);
            }

            // trim whitespace from end of text
            text = text.TrimEnd();

            return text;
        }

        #endregion
    }
}