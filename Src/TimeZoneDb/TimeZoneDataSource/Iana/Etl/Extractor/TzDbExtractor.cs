using System;
using System.Collections.Generic;
using System.Linq;
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

            // a list of files which contain data
            // see ftp://ftp.iana.org/tz/code/Makefile
            var dataFileNameList = new []
            {
                // YDATA
                "africa",
                "antarctica",
                "asia",
                "australasia",
                "europe",
                "northamerica",
                "southamerica",
                "pacificnew",
                "etcetera",
                "backward",
                // TABDATA
                "zone.tab",
                // OTHER
                "backzone"
            };

            foreach (var dataFileName in dataFileNameList)
            {
                var dataFile =
                    _fileSource.SingleOrDefault(
                        file =>
                            file.Name.Equals(
                                dataFileName,
                                StringComparison.InvariantCultureIgnoreCase));

                if (null != dataFile)
                {
                    _parser.Parse(dataFile,_extractorDatabase);
                }
            }

            return new ExtractorDatabaseReader(_extractorDatabase);
        }

        #endregion
    }
}