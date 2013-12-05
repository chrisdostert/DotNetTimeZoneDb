using System;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer
{
    public struct LineOfFile
    {
        #region Ctors

        public LineOfFile(int lineNumber, String lineText) : this()
        {
            LineNumber = lineNumber;
            LineText = lineText;
        }

        #endregion

        #region Properties

        public int LineNumber { get; private set; }
        public String LineText { get; private set; }

        #endregion
    }
}