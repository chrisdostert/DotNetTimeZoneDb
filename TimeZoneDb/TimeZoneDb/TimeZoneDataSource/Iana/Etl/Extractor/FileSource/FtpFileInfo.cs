using System;
using System.IO;
using System.Net;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    public class FtpFileInfo : IFileInfo
    {
        #region Ctors

        public FtpFileInfo(String name, Uri fileUri, NetworkCredential networkCredential = null)
        {
            Name = name;
            _fileUri = fileUri;
            _networkCredential = networkCredential ?? new NetworkCredential("anonymous", "");
        }

        #endregion

        #region Fields

        private readonly Uri _fileUri;
        private readonly NetworkCredential _networkCredential;

        #endregion

        #region Methods

        public string Name { get; private set; }

        public StreamReader StreamReader
        {
            get
            {
                var request = (FtpWebRequest) WebRequest.Create(_fileUri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = _networkCredential;

                var response = (FtpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                return new StreamReader(responseStream);
            }
        }

        #endregion
    }
}