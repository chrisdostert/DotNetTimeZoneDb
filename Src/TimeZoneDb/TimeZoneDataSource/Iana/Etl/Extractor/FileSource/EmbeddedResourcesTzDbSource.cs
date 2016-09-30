using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    public class EmbeddedResourcesTzDbSource : ITzDbFileSource
    {
        private Assembly _assembly;

        private const string NAME_SPACE = "TimeZoneDb.TimeZoneDataSource.tzdatabase.";

        private static string[] dataFileNameList = new[]
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

        public IEnumerator<IFileInfo> GetEnumerator()
        {      
            return GetFileInfos().GetEnumerator();
        }

        public IList<IFileInfo> GetFileInfos()
        {
           IList<IFileInfo> localFileinfos = new List<IFileInfo>();

            _assembly = Assembly.GetExecutingAssembly();

            foreach (var fileName in dataFileNameList)
            {
                StreamReader streamReader = new StreamReader(_assembly.GetManifestResourceStream(NAME_SPACE + fileName));
                localFileinfos.Add(new EmbeddedResourceFileInfo() { Name = fileName, StreamReader = streamReader });
            }

            return localFileinfos;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
