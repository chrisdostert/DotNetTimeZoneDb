using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.Entities
{
    /// <summary>
    ///     An <see cref="AbstractDaylightSavingsAdjustment" /> implementation which uses rules to determine
    ///     the offset adjustment
    /// </summary>
    public class RuleBasedDaylightSavingsAdjustment : AbstractDaylightSavingsAdjustment
    {
        #region Ctors

        public RuleBasedDaylightSavingsAdjustment(String name, IEnumerable<DaylightSavingsRule> daylightSavingsRules,
            Guid? id = null)
            : base(id)
        {
            Name = name;
            DaylightSavingsRules = daylightSavingsRules;
        }

        #endregion

        #region Properties

        public String Name { get; private set; }
        public IEnumerable<DaylightSavingsRule> DaylightSavingsRules { get; private set; }

        #endregion

        #region Methods

        public override TimeSpan GetAdjustmentToStandardOffset(TimeSpan standardOffset, Moment moment)
        {
            DaylightSavingsRule effectiveRule;
            if (TryGetMostRecentDaylightSavingsRule(standardOffset, moment, out effectiveRule))
            {
                return effectiveRule.AdjustmentToStandardOffset;
            }

            return TimeSpan.Zero;
        }

        public override string GetTimeZoneAbbreviation(TimeSpan standardOffset, TimeZoneAbbreviationFormat format,
            Moment moment)
        {
            DaylightSavingsRule effectiveRule;
            if (TryGetMostRecentDaylightSavingsRule(standardOffset, moment, out effectiveRule))
            {
                return String.Format(format.Format, effectiveRule.TimeZoneAbbreviationVariable.Value);
            }

            return format.Format;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        ///     A utility method to retrieve all <see cref="DaylightSavingsRule" />s that are in effect within
        ///     a given <paramref name="year" />
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private IEnumerable<DaylightSavingsRule> GetAllDaylightSavingsRulesEffectiveInYear(int year)
        {
            var effectiveDaylightSavingsRules = new List<DaylightSavingsRule>();

            foreach (DaylightSavingsRule daylightSavingsRule in DaylightSavingsRules)
            {
                int ruleEffectiveFrom = daylightSavingsRule.UtcEffectiveFromSpecification.ToYear();
                int ruleEffectiveTo = daylightSavingsRule.UtcEffectiveToSpecification.ToYear();

                // handle rules effective during the year the moment we're asking about occurs
                if (ruleEffectiveFrom <= year && year <= ruleEffectiveTo)
                {
                    effectiveDaylightSavingsRules.Add(daylightSavingsRule);
                }
            }

            return effectiveDaylightSavingsRules;
        }

        /// a utility method that tries to get the first
        /// <see cref="DaylightSavingsRule" />
        /// that will/have occur(ed)
        /// Note: returns false if none ever occurred
        private bool TryGetEarliestYearWithEffectiveDaylightSavingsRules(out int? earliestYearWithDaylightSavingsRules)
        {
            earliestYearWithDaylightSavingsRules = (from daylightSavingsRule in DaylightSavingsRules
                orderby daylightSavingsRule.UtcEffectiveFromSpecification.ToYear()
                select daylightSavingsRule.UtcEffectiveFromSpecification.ToYear())
                .FirstOrDefault();

            if (null != earliestYearWithDaylightSavingsRules)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     A utility method to order <see cref="DaylightSavingsRule" />s by the day they occur
        ///     in a given <paramref name="year" />
        /// </summary>
        /// <param name="daylightSavingsRules"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private IEnumerable<DaylightSavingsRule> OrderRulesByDayOfYear(
            IEnumerable<DaylightSavingsRule> daylightSavingsRules, int year)
        {
            return daylightSavingsRules.OrderBy(daylightSavingsRule => daylightSavingsRule,
                new DaylightSavingsRuleComparer(year));
        }

        /// <summary>
        ///     Gets the <see cref="DaylightSavingsRule" /> in effect at a particular <paramref name="moment" />
        /// </summary>
        /// <param name="standardOffset"></param>
        /// <param name="moment"></param>
        /// <param name="mostRecentDaylightSavingsRule"></param>
        /// <returns></returns>
        private bool TryGetMostRecentDaylightSavingsRule(TimeSpan standardOffset, Moment moment,
            out DaylightSavingsRule mostRecentDaylightSavingsRule)
        {
            mostRecentDaylightSavingsRule = null;
            DateTime dateTimeOfMoment = moment.ToDateTime();

            TimeSpan mostRecentOffsetAdjustment = TimeSpan.Zero;
            DaylightSavingsRule mostRecentDaylightSavingsRulePriorToYear;
            if (TryGetMostRecentDaylightSavingsRulePriorToYear(dateTimeOfMoment.Year,
                out mostRecentDaylightSavingsRulePriorToYear))
            {
                mostRecentOffsetAdjustment = mostRecentDaylightSavingsRulePriorToYear.AdjustmentToStandardOffset;
                mostRecentDaylightSavingsRule = mostRecentDaylightSavingsRulePriorToYear;
            }

            DaylightSavingsRule mostRecentDaylightSavingsRuleDuringYear;
            if (TryGetMostRecentDaylightSavingsRuleDuringYear(standardOffset, mostRecentOffsetAdjustment, moment,
                out mostRecentDaylightSavingsRuleDuringYear))
            {
                mostRecentDaylightSavingsRule = mostRecentDaylightSavingsRuleDuringYear;
            }

            if (null == mostRecentDaylightSavingsRule)
            {
                return false;
            }
            return true;
        }

        private bool TryGetMostRecentDaylightSavingsRuleDuringYear(TimeSpan standardOffset,
            TimeSpan mostRecentAdjustmentToStandardOffset, Moment moment,
            out DaylightSavingsRule mostRecentDaylightSavingsRule)
        {
            mostRecentDaylightSavingsRule = null;
            DateTime dateTimeOfMoment = moment.ToDateTime();

            // get the DaylightSavingsRules from the current year.
            IEnumerable<DaylightSavingsRule> currentYearDaylightSavingsRules =
                GetAllDaylightSavingsRulesEffectiveInYear(dateTimeOfMoment.Year);

            // Guard: current year has no rules
            if (!currentYearDaylightSavingsRules.Any())
            {
                return false;
            }

            var momentRulesDictionary = new Dictionary<Moment, DaylightSavingsRule>();
            IEnumerable<DaylightSavingsRule> orderedCurrentYearRules =
                OrderRulesByDayOfYear(currentYearDaylightSavingsRules,
                    dateTimeOfMoment.Year);

            // initialize offset to offset at end of previous year
            TimeSpan effectiveAdjustmentToStandardOffset = mostRecentAdjustmentToStandardOffset;

            // determine the Moment each DaylightSavingsRule occurs 
            foreach (DaylightSavingsRule daylightSavingsRule in orderedCurrentYearRules)
            {
                Moment momentDaylightSavingsRuleOccurs = daylightSavingsRule.ToMoment(standardOffset,
                    effectiveAdjustmentToStandardOffset, dateTimeOfMoment.Year);
                momentRulesDictionary.Add(momentDaylightSavingsRuleOccurs, daylightSavingsRule);

                // update daylight savings adjustment
                effectiveAdjustmentToStandardOffset = daylightSavingsRule.AdjustmentToStandardOffset;
            }

            // locate the most recent rule to have occured this year
            int i = 0;
            while (momentRulesDictionary.Count > i && momentRulesDictionary.ElementAt(i).Key < moment)
            {
                mostRecentDaylightSavingsRule = momentRulesDictionary.ElementAt(i).Value;
                i++;
            }

            return true;
        }


        private bool TryGetMostRecentDaylightSavingsRulePriorToYear(int year,
            out DaylightSavingsRule daylightSavingsRuleInEffectAtStartOfYear)
        {
            // get the earliest year any rule ever was in effect
            int? earliestYearWithEffectiveDaylightSavingsRules;

            // Guard: no rule was ever in effect
            if (!TryGetEarliestYearWithEffectiveDaylightSavingsRules(out earliestYearWithEffectiveDaylightSavingsRules))
            {
                daylightSavingsRuleInEffectAtStartOfYear = null;
                return false;
            }

            // Guard: earliest year any DaylightSavingsRule was in effect is on or after the year we're 
            // looking to start
            if (earliestYearWithEffectiveDaylightSavingsRules.Value >= year)
            {
                daylightSavingsRuleInEffectAtStartOfYear = null;
                return false;
            }

            // initialize loop variables
            int previousYear = year - 1;
            IEnumerable<DaylightSavingsRule> previousYearDaylightSavingsRules =
                GetAllDaylightSavingsRulesEffectiveInYear(previousYear);

            // walk backwards through the DaylightSavingsRules until we find the most recent year
            // in which one or more rules were active
            while (!previousYearDaylightSavingsRules.Any())
            {
                previousYear--;
                previousYearDaylightSavingsRules = GetAllDaylightSavingsRulesEffectiveInYear(previousYear);
            }

            daylightSavingsRuleInEffectAtStartOfYear =
                OrderRulesByDayOfYear(previousYearDaylightSavingsRules, previousYear).Last();
            return true;
        }

        #endregion
    }
}