using System;
using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor
{
    public class TzDbExtractor : ITzDbExtractor
    {
        #region Ctors

        /// <summary>
        /// </summary>
        /// <param name="fileSource">defaults to an <see cref="FtpTzDbFileSource" /></param>
        /// <param name="database">defaults to an <see cref="InMemoryExtractorDatabase" /></param>
        /// <param name="parser">defaults to an <see cref="TzDbParser" /></param>
        public TzDbExtractor(ITzDbFileSource fileSource = null, IExtractorDatabase database = null,
            ITzDbParser parser = null)
        {
            _fileSource = fileSource ?? new FtpTzDbFileSource();
            _extractorDatabase = database ?? new InMemoryExtractorDatabase();
            _parser = parser ?? new TzDbParser();
        }

        #endregion

        #region Fields

        private readonly IExtractorDatabase _extractorDatabase;

        private readonly ITzDbFileSource _fileSource;
        private readonly ITzDbParser _parser;

        #endregion

        #region Methods

        public IExtractorDatabaseReader Extract()
        {
            // start with a clean slate (in case this is an update rather than an initial extraction)
            _extractorDatabase.DropAndReCreate();

            // a list of files this extraction doesnt care about
            var ignoreList = new List<String>
            {
                "factory",
                "iso3166.tab",
                "leapseconds",
                "leapseconds.awk",
                "leap-seconds.list",
                "Makefile",
                "README",
                "solar87",
                "solar88",
                "solar89",
                "systemv",
                "yearistype.sh"
            };

            foreach (IFileInfo file in _fileSource)
            {
                // Performance enhancement: ignore files that aren't relevant
                if (ignoreList.Contains(file.Name))
                {
                    continue;
                }

                _parser.Parse(file, _extractorDatabase);
            }

            return new ExtractorDatabaseReader(_extractorDatabase);
        }

        #endregion
    }
}