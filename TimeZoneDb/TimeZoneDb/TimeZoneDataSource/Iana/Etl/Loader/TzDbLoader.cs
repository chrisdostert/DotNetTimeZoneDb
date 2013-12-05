using System.Linq;
using TimeZoneDb.Repositories;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Loader
{
    public class TzDbLoader : ITzDbLoader
    {
        #region Ctors

        public TzDbLoader(ITzDbTransformer tzDbTransformer = null)
        {
            _tzDbTransformer = tzDbTransformer ?? new TzDbTransformer();
        }

        #endregion

        #region Fields

        private readonly ITzDbTransformer _tzDbTransformer;

        #endregion

        #region Methods

        public void Load(ITimeZoneRepository timeZoneRepository,
            IDaylightSavingsAdjustmentRepository daylightSavingsAdjustmentRepository)
        {
            ITransformerDatabaseReader transformerDatabaseReader = _tzDbTransformer.Transform();

            transformerDatabaseReader.OffsetAdjustments.ToList()
                .ForEach(daylightSavingsAdjustmentRepository.Add);

            transformerDatabaseReader.TimeZones.ToList()
                .ForEach(timeZoneRepository.Add);
        }

        #endregion
    }
}