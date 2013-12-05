using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser
{
    /// <summary>
    ///     A Top Down Parser for TzDbFiles
    /// </summary>
    public class TzDbParser : ITzDbParser
    {
        #region Ctors

        /// <summary>
        ///     Initializes a new <see cref="TzDbParser" />
        /// </summary>
        /// <param name="tokenizer">defaults to <see cref="TzDbTokenizer" /></param>
        public TzDbParser(ITzDbTokenizer tokenizer = null)
        {
            _tokenizer = tokenizer ?? new TzDbTokenizer();
        }

        #endregion

        #region Fields

        private readonly ITzDbTokenizer _tokenizer;

        #endregion

        #region Methods

        /// <summary>
        ///     Parses the <paramref name="fileInfo" /> and adds parsed types to the
        ///     <paramref name="extractorDatabase" />
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="extractorDatabase"></param>
        public void Parse(IFileInfo fileInfo, IExtractorDatabase extractorDatabase)
        {
            Queue<Token> unProcessedTokens = _tokenizer.Tokenize(fileInfo);

            Token beginFileToken = unProcessedTokens.Dequeue();

            // guard first token received is start of file token
            if (beginFileToken.TokenType != TokenType.BeginFile)
            {
                string msg = String.Format("received unexpected token type:{0} when token type should have been {1}",
                    beginFileToken.TokenType, TokenType.BeginFile);
                throw new Exception(msg);
            }

            while (unProcessedTokens.Any())
            {
                Token firstToken = unProcessedTokens.Dequeue();

                // handle end of file
                if (firstToken.TokenType == TokenType.EndFile)
                {
                    break;
                }

                // guard first token received is start of line token
                if (firstToken.TokenType != TokenType.BeginLine)
                {
                    string msg = String.Format(
                        "received unexpected token type:{0} when token type should have been {1}", firstToken.TokenType,
                        TokenType.BeginLine);
                    throw new Exception(msg);
                }

                TzDbLink? tzDbLink;
                if (LinkExpression.TryParse(unProcessedTokens, out tzDbLink))
                {
                    extractorDatabase.Links.Add(tzDbLink.Value);
                    continue;
                }

                TzDbZoneDescription tzDbZoneDescription;
                if (ZoneDescriptionExpression.TryParse(unProcessedTokens, out tzDbZoneDescription))
                {
                    extractorDatabase.ZoneDescriptions.Add(tzDbZoneDescription);
                    continue;
                }

                TzDbZoneDefinition tzDbZoneDefinition;
                if (ZoneDefinitionExpression.TryParse(unProcessedTokens, out tzDbZoneDefinition))
                {
                    extractorDatabase.ZoneDefinitions.Add(tzDbZoneDefinition);
                    continue;
                }

                TzDbRule? tzDbRule;
                if (RuleExpression.TryParse(unProcessedTokens, out tzDbRule))
                {
                    extractorDatabase.Rules.Add(tzDbRule.Value);
                    continue;
                }

                string unsupportedTokenMsg = String.Format("received unsupported tokens on line: {0} of file: {1}",
                    firstToken.Value, fileInfo.Name);
                throw new Exception(unsupportedTokenMsg);
            }
        }

        #endregion
    }
}