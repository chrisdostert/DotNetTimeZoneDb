using System;
using System.IO;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    public interface IFileInfo
    {
        #region Properties

        /// <summary>
        ///     The Name of the file
        /// </summary>
        String Name { get; }

        /// <summary>
        ///     Returns a <see cref="StreamReader" /> for the file
        /// </summary>
        StreamReader StreamReader { get; }

        #endregion
    }
}