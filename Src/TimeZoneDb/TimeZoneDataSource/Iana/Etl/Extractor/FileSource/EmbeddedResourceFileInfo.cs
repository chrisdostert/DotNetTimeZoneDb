using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.FileSource
{
    public class EmbeddedResourceFileInfo : IFileInfo
    {
        public string Name { get; set; }
        public StreamReader StreamReader { get; set;}
    }
}
