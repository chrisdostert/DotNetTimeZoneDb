using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    /// <summary>
    ///     A source for TzDb files that uses an ftp directory
    /// </summary>
    public class FtpTzDbFileSource : ITzDbFileSource
    {
        #region Ctors

        public FtpTzDbFileSource(Uri ftpFileDirectoryUri = null, NetworkCredential networkCredential = null)
        {
            _ftpFileDirectoryUri = ftpFileDirectoryUri ?? new Uri("ftp://ftp.iana.org/tz/data/");
            _networkCredential = networkCredential ?? new NetworkCredential("anonymous", "");
        }

        #endregion

        #region Fields

        private readonly Uri _ftpFileDirectoryUri;
        private readonly NetworkCredential _networkCredential;

        #endregion

        #region Methods

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            var request = (FtpWebRequest) WebRequest.Create(_ftpFileDirectoryUri);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = _networkCredential;

            var response = (FtpWebResponse) request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream);

            var fileInfoList = new List<IFileInfo>();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();

                // construct the FtpFileInfo object for this line
                string name = line;
                var fileUri = new Uri(_ftpFileDirectoryUri, name);
                var fileInfo = new FtpFileInfo(line, fileUri, _networkCredential);

                fileInfoList.Add(fileInfo);
            }
            return fileInfoList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}