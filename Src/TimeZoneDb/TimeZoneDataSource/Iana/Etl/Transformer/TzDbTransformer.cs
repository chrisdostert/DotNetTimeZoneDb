using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.Entities;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Database;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Transformer
{
    public class TzDbTransformer : ITzDbTransformer
    {
        #region Ctors

        public TzDbTransformer(ITzDbExtractor tzDbExtractor = null, ITransformerDatabase transformerDatabase = null)
        {
            _tzDbExtractor = tzDbExtractor ?? new TzDbExtractor();
            _transformerDatabase = transformerDatabase ?? new InMemoryTransformerDatabase();
        }

        #endregion

        #region Fields

        private readonly ITransformerDatabase _transformerDatabase;
        private readonly ITzDbExtractor _tzDbExtractor;

        #endregion

        #region Methods

        public ITransformerDatabaseReader Transform()
        {
            IExtractorDatabaseReader extractorDatabaseReader = _tzDbExtractor.Extract();

            // start with a clean slate (in case this is an update rather than an initial extraction)
            _transformerDatabase.DropAndReCreate();

            // group the rules by name
            IEnumerable<IGrouping<string, TzDbRule>> tzDbRuleGroups =
                extractorDatabaseReader.TzDbRules.GroupBy(tzDbRule => tzDbRule.Name);


            foreach (TzDbZoneDefinition tzDbZoneDefinition in extractorDatabaseReader.TzDbZoneDefinitions)
            {
                Guid timeZoneId = Guid.NewGuid();
                IEnumerable<TimeZoneImplementation> timeZoneImplementations = GetTimeZoneImplementations(timeZoneId,
                    tzDbZoneDefinition, tzDbRuleGroups);
                IEnumerable<TimeZoneAlias> aliases = FindTimeZoneAliases(tzDbZoneDefinition,
                    extractorDatabaseReader.TzDbLinks);
                TzDbZoneDescription description = FindTzDbZoneDescription(tzDbZoneDefinition,
                    extractorDatabaseReader.TzDbZoneDescriptions, aliases);

                if (null != description)
                {
                    // create a DbTimeZone from this ZoneDefinition
                    _transformerDatabase.TimeZones.Add(tzDbZoneDefinition.ToTimeZone(timeZoneId, description.Coordinates,
                        description.CountryCode, aliases, timeZoneImplementations));
                    continue;
                }

                // create a DbTimeZone from this ZoneDefinition
                _transformerDatabase.TimeZones.Add(tzDbZoneDefinition.ToTimeZone(timeZoneId, null, null, aliases,
                    timeZoneImplementations));
            }

            return new TransformerDatabaseReader(_transformerDatabase);
        }

        private TzDbZoneDescription FindTzDbZoneDescription(TzDbZoneDefinition tzDbZoneDefinition,
            IEnumerable<TzDbZoneDescription> tzDbZoneDescriptions, IEnumerable<TimeZoneAlias> aliases)
        {
            TzDbZoneDescription description =
                tzDbZoneDescriptions.SingleOrDefault(
                    tzDbZoneDescription => tzDbZoneDescription.Tz == tzDbZoneDefinition.Zone.Name);
            if (null == description)
            {
                tzDbZoneDescriptions.SingleOrDefault(
                    tzDbZoneDescription => aliases.Any(alias => alias.Name == tzDbZoneDescription.Tz));
            }

            return description;
        }

        private IEnumerable<TimeZoneAlias> FindTimeZoneAliases(TzDbZoneDefinition tzDbZoneDefinition,
            IEnumerable<TzDbLink> tzDbLinks)
        {
            return
                tzDbLinks.Where(tzDbLink => tzDbLink.LinkFrom == tzDbZoneDefinition.Zone.Name)
                    .Select(tzDbLink => tzDbLink.ToTimeZoneAlias());
        }

        private IEnumerable<TimeZoneImplementation> GetTimeZoneImplementations(Guid timeZoneId,
            TzDbZoneDefinition tzDbZoneDefinition, IEnumerable<IGrouping<string, TzDbRule>> tzDbRuleGroups)
        {
            // get the daylightSavingsAdjustment for the current zone
            AbstractDaylightSavingsAdjustment offsetAdjustment =
                FindOrCreateDaylightSavingsAdjustment(tzDbZoneDefinition.Zone.Rules, tzDbRuleGroups);

            var timeZoneImplementations = new List<TimeZoneImplementation>
            {
                tzDbZoneDefinition.Zone.ToTimeZoneImplementation(offsetAdjustment)
            };

            // process the zone continuations for the current zone
            foreach (TzDbZoneContinuation tzDbZoneContinuation in tzDbZoneDefinition.ContinuationLines)
            {
                AbstractDaylightSavingsAdjustment zoneContinuationOffsetAdjustment =
                    FindOrCreateDaylightSavingsAdjustment(tzDbZoneContinuation.Rules, tzDbRuleGroups);

                // create a TimeZoneImplementation from this zone definitions zone continuation
                timeZoneImplementations.Add(
                    tzDbZoneContinuation.ToTimeZoneImplementation(zoneContinuationOffsetAdjustment));
            }

            return timeZoneImplementations;
        }

        /// <summary>
        ///     A Helper to attempt to find a <see cref="AbstractDaylightSavingsAdjustment" /> with a given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private AbstractDaylightSavingsAdjustment FindOrCreateDaylightSavingsAdjustment(string name,
            IEnumerable<IGrouping<string, TzDbRule>> tzDbRuleGroups)
        {
            if (name != "-")
            {
                // handle ExplicitOffsetAdjustments
                TimeSpan adjustment;
                if (TimeSpan.TryParse(name, out adjustment))
                {
                    var explicitOffsetAdjustment = new ExplicitDaylightSavingsAdjustment(adjustment);
                    _transformerDatabase.DaylightSavingsAdjustments.Add(explicitOffsetAdjustment);
                    return explicitOffsetAdjustment;
                }

                // if RuleBasedDaylightSavingsAdjustment has already been added just retrieve it
                RuleBasedDaylightSavingsAdjustment existingRuleBasedOffsetAdjustment =
                    _transformerDatabase.DaylightSavingsAdjustments.OfType<RuleBasedDaylightSavingsAdjustment>()
                        .SingleOrDefault(_ruleBasedOffsetAdjustment => _ruleBasedOffsetAdjustment.Name == name);

                if (null != existingRuleBasedOffsetAdjustment)
                {
                    return existingRuleBasedOffsetAdjustment;
                }

                // otherwise we need to create the RuleBasedDaylightSavingsAdjustment
                IGrouping<string, TzDbRule> tzDbRuleGroup =
                    tzDbRuleGroups.Single(_tzDbRuleGroup => _tzDbRuleGroup.Key == name);
                // build the list of DaylightSavingsRules for this RuleBasedDaylightSavingsAdjustment
                var offsetAdjustmentRules = new List<DaylightSavingsRule>();
                foreach (TzDbRule tzDbRule in tzDbRuleGroup)
                {
                    offsetAdjustmentRules.Add(tzDbRule.ToOffsetAdjustmentSpecification());
                }
                var ruleBasedOffsetAdjustment = new RuleBasedDaylightSavingsAdjustment(tzDbRuleGroup.Key,
                    offsetAdjustmentRules);
                _transformerDatabase.DaylightSavingsAdjustments.Add(ruleBasedOffsetAdjustment);
                return ruleBasedOffsetAdjustment;
            }
            return null;
        }

        #endregion
    }
}