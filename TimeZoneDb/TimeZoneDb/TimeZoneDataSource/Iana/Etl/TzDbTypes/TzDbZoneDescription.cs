using System;
using System.Spatial;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public class TzDbZoneDescription
    {
        #region Ctors

        public TzDbZoneDescription(String countryCode, GeographyPoint coordinates, String tz, String comments = null)
        {
            CountryCode = countryCode;
            Coordinates = coordinates;
            Tz = tz;
            Comments = comments;
        }

        #endregion

        #region Properties

        public String CountryCode { get; private set; }
        public GeographyPoint Coordinates { get; private set; }
        public String Tz { get; private set; }
        public String Comments { get; private set; }

        #endregion
    }
}