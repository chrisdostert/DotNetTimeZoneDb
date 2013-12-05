using System;
using System.Collections.Generic;
using System.Spatial;
using TimeZoneDb.Entities;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    /// <summary>
    ///     A zone definition is unique from any other entry in the timezone database
    ///     in that it is defined by a zone line immediately proceeded by one or more
    ///     continuation lines. The relationship is maintained only by this proximity,
    ///     i.e. the continuation lines contain no foreign key to the zone
    ///     for which they continue. Thus, unless held in the same datastructure as
    ///     the particular zone they would lose their association
    /// </summary>
    public class TzDbZoneDefinition
    {
        #region Ctors

        public TzDbZoneDefinition(TzDbZone zone, List<TzDbZoneContinuation> continuationLines)
        {
            Zone = zone;
            ContinuationLines = continuationLines ?? new List<TzDbZoneContinuation>();
        }

        #endregion

        #region Properties

        public TzDbZone Zone { get; private set; }
        public List<TzDbZoneContinuation> ContinuationLines { get; private set; }

        #endregion

        #region Conversion Methods

        public DbTimeZone ToTimeZone(Guid timeZoneId, GeographyPoint coordinates, String iso31661Alpha2,
            IEnumerable<TimeZoneAlias> aliases,
            IEnumerable<TimeZoneImplementation> implementationSpecifications)
        {
            return new DbTimeZone(Zone.Name, coordinates, iso31661Alpha2, aliases, implementationSpecifications,
                timeZoneId);
        }

        #endregion
    }
}