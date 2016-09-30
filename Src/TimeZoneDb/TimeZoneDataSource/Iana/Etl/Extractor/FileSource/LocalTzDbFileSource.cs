using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    /// <summary>
    ///     A source for TzDb files that uses a local file directory
    /// </summary>
    public class LocalTzDbFileSource : ITzDbFileSource
    {
        #region Ctors

        public LocalTzDbFileSource(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        #endregion

        #region Fields

        private readonly DirectoryInfo _directoryInfo;

        #endregion

        #region Methods

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return GetFileInfos().GetEnumerator();
        }

        public IList<IFileInfo> GetFileInfos()
        {
            IEnumerable<FileInfo> fileInfos = _directoryInfo.EnumerateFiles();
            var localFileinfos = new List<IFileInfo>();
            foreach (FileInfo fileInfo in fileInfos)
            {
                localFileinfos.Add(new LocalFileInfo(fileInfo));
            }
            return localFileinfos;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}