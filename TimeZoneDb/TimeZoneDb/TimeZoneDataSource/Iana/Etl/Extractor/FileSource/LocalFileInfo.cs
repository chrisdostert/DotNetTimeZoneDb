using System.IO;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    public class LocalFileInfo : IFileInfo
    {
        #region Ctors

        public LocalFileInfo(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        #endregion

        #region Fields

        private readonly FileInfo _fileInfo;

        #endregion

        #region Methods

        public string Name
        {
            get { return _fileInfo.Name; }
        }

        public StreamReader StreamReader
        {
            get { return new StreamReader(_fileInfo.OpenRead()); }
        }

        #endregion
    }
}