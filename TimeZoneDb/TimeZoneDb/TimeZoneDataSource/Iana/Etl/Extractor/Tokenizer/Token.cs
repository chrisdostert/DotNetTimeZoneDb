using System;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer
{
    /// <summary>
    ///     Represents a <see cref="TzDbTokenizer" /> token
    /// </summary>
    public struct Token
    {
        #region Ctors

        public Token(TokenType type, string value)
            : this()
        {
            Value = value;
            TokenType = type;
        }

        #endregion

        #region Properties

        public String Value { get; private set; }
        public TokenType TokenType { get; private set; }

        #endregion
    }
}