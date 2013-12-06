using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class DayOfMonthSpecificationExpression
    {
        private const String MondayRegex = "(Mon)|(Monday)";
        private const String TuesdayRegex = "(Tue)|(Tuesday)";
        private const String WednesdayRegex = "(Wed)|(Wednesday)";
        private const String ThursdayRegex = "(Thu)|(Thursday)";
        private const String FridayRegex = "(Fri)|(Friday)";
        private const String SaturdayRegex = "(Sat)|(Saturday)";
        private const String SundayRegex = "(Sun)|(Sunday)";

        private static readonly String DaysOfWeekRegex = String.Format("({0}|{1}|{2}|{3}|{4}|{5}|{6})", MondayRegex,
            TuesdayRegex, WednesdayRegex, ThursdayRegex, FridayRegex, SaturdayRegex, SundayRegex);

        public static AbstractDayOfMonthSpecification Parse(Queue<Token> unProcessedTokens)
        {
            AbstractDayOfMonthSpecification dayOfMonthSpecification;
            if (!TryParse(unProcessedTokens, out dayOfMonthSpecification))
            {
                string msg = String.Format("Unable to parse day of month specification expression: {0}",
                    unProcessedTokens.Peek().Value);
                throw new Exception(msg);
            }

            return dayOfMonthSpecification;
        }

        public static bool TryParse(Queue<Token> unProcessedTokens,
            out AbstractDayOfMonthSpecification dayOfMonthSpecification)
        {
            if (TryGetDayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(unProcessedTokens,
                out dayOfMonthSpecification))
            {
                return true;
            }
            if (TryGetDayOfWeekLessThanOrEqualToDayOfMonthSpecification(unProcessedTokens,
                out dayOfMonthSpecification))
            {
                return true;
            }
            if (TryGetExplicitDayOfMonthSpecification(unProcessedTokens, out dayOfMonthSpecification))
            {
                return true;
            }
            if (TryGetLastWeekdayOfMonthSpecification(unProcessedTokens, out dayOfMonthSpecification))
            {
                return true;
            }

            dayOfMonthSpecification = null;
            return false;
        }

        private static bool TryGetExplicitDayOfMonthSpecification(Queue<Token> unProcessedTokens,
            out AbstractDayOfMonthSpecification dayOfMonthSpecification)
        {
            const String explicitDayOfMonthRegex = @"^\d{1,2}$";

            // check if the next token is an explicit day of month specification token
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, explicitDayOfMonthRegex))
            {
                string dayOfMonthSpecificationString = unProcessedTokens.Dequeue().Value;

                dayOfMonthSpecification = new ExplicitDayOfMonthSpecification(int.Parse(dayOfMonthSpecificationString));
                return true;
            }
            dayOfMonthSpecification = null;
            return false;
        }

        private static bool TryGetDayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(
            Queue<Token> unProcessedTokens, out AbstractDayOfMonthSpecification dayOfMonthSpecification)
        {
            const String weekdayGreaterThanOrEqualToDayRegexFormat = @"^{0}>=\d{{1,2}}$";
            string weekdayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                DaysOfWeekRegex);
            string mondayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                MondayRegex);
            string tuesdayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                TuesdayRegex);
            string wednesdayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                WednesdayRegex);
            string thursdayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                ThursdayRegex);
            string fridayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                FridayRegex);
            string saturdayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                SaturdayRegex);
            string sundayGreaterThanOrEqualToDayRegex = String.Format(weekdayGreaterThanOrEqualToDayRegexFormat,
                SundayRegex);

            // check if the next token is a day of week greater than or equal to day of month specification token
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, weekdayGreaterThanOrEqualToDayRegex))
            {
                string dayOfMonthSpecificationString = unProcessedTokens.Dequeue().Value;

                if (Regex.IsMatch(dayOfMonthSpecificationString, mondayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(DayOfWeek.Monday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, tuesdayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(
                        DayOfWeek.Tuesday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, wednesdayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(
                        DayOfWeek.Wednesday, dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, thursdayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification =
                        new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(DayOfWeek.Thursday,
                            dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, fridayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(DayOfWeek.Friday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, saturdayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification =
                        new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(DayOfWeek.Saturday,
                            dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, sundayGreaterThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekGreaterThanOrEqualToDayOfMonthSpecification(DayOfWeek.Sunday,
                        dayOfMonth);
                    return true;
                }

                string msg =
                    String.Format(
                        "Unable to parse day of week greater than or equal to day of month specification expression: {0}",
                        dayOfMonthSpecificationString);
                throw new Exception(msg);
            }
            dayOfMonthSpecification = null;
            return false;
        }

        private static bool TryGetDayOfWeekLessThanOrEqualToDayOfMonthSpecification(
            Queue<Token> unProcessedTokens, out AbstractDayOfMonthSpecification dayOfMonthSpecification)
        {
            const String weekdayLessThanOrEqualToDayRegexFormat = @"^{0}<=\d{{1,2}}$";
            string weekdayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat,
                DaysOfWeekRegex);
            string mondayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat, MondayRegex);
            string tuesdayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat, TuesdayRegex);
            string wednesdayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat,
                WednesdayRegex);
            string thursdayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat,
                ThursdayRegex);
            string fridayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat, FridayRegex);
            string saturdayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat,
                SaturdayRegex);
            string sundayLessThanOrEqualToDayRegex = String.Format(weekdayLessThanOrEqualToDayRegexFormat, SundayRegex);

            // check if the next token is a day of week less than or equal to day of month specification token
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, weekdayLessThanOrEqualToDayRegex))
            {
                string dayOfMonthSpecificationString = unProcessedTokens.Dequeue().Value;

                if (Regex.IsMatch(dayOfMonthSpecificationString, mondayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(DayOfWeek.Monday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, tuesdayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(DayOfWeek.Tuesday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, wednesdayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(
                        DayOfWeek.Wednesday, dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, thursdayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(DayOfWeek.Thursday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, fridayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(DayOfWeek.Friday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, saturdayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(DayOfWeek.Saturday,
                        dayOfMonth);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, sundayLessThanOrEqualToDayRegex))
                {
                    int dayOfMonth = ParseDayOfMonthFromInequality(dayOfMonthSpecificationString);
                    dayOfMonthSpecification = new DayOfWeekLessThanOrEqualToDayOfMonthSpecification(DayOfWeek.Sunday,
                        dayOfMonth);
                    return true;
                }

                string msg =
                    String.Format(
                        "Unable to parse day of week less than or equal to day of month specification expression: {0}",
                        dayOfMonthSpecificationString);
                throw new Exception(msg);
            }
            dayOfMonthSpecification = null;
            return false;
        }

        /// <summary>
        ///     A helper method to parse the day of month out of a day of month inequality specification expression
        /// </summary>
        /// <param name="dayOfMonthSpecificationString"></param>
        /// <returns></returns>
        private static int ParseDayOfMonthFromInequality(String dayOfMonthSpecificationString)
        {
            string dayOfMonthSubstring =
                dayOfMonthSpecificationString.Substring(dayOfMonthSpecificationString.IndexOf('=') + 1);
            return int.Parse(dayOfMonthSubstring);
        }

        private static bool TryGetLastWeekdayOfMonthSpecification(Queue<Token> unProcessedTokens,
            out AbstractDayOfMonthSpecification dayOfMonthSpecification)
        {
            const String lastDayOfWeekRegexFormat = @"^last{0}$";
            string lastDayOfWeekRegex = String.Format(lastDayOfWeekRegexFormat, DaysOfWeekRegex);
            string lastMondayRegex = String.Format(lastDayOfWeekRegexFormat, MondayRegex);
            string lastTuesdayRegex = String.Format(lastDayOfWeekRegexFormat, TuesdayRegex);
            string lastWednesdayRegex = String.Format(lastDayOfWeekRegexFormat, WednesdayRegex);
            string lastThursdayRegex = String.Format(lastDayOfWeekRegexFormat, ThursdayRegex);
            string lastFridayRegex = String.Format(lastDayOfWeekRegexFormat, FridayRegex);
            string lastSaturdayRegex = String.Format(lastDayOfWeekRegexFormat, SaturdayRegex);
            string lastSundayRegex = String.Format(lastDayOfWeekRegexFormat, SundayRegex);

            // check if the next token is a last weekday of month specification token
            if (Regex.IsMatch(unProcessedTokens.Peek().Value, lastDayOfWeekRegex))
            {
                string dayOfMonthSpecificationString = unProcessedTokens.Dequeue().Value;

                if (Regex.IsMatch(dayOfMonthSpecificationString, lastMondayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Monday);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, lastTuesdayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Tuesday);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, lastWednesdayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Wednesday);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, lastThursdayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Thursday);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, lastFridayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Friday);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, lastSaturdayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Saturday);
                    return true;
                }
                if (Regex.IsMatch(dayOfMonthSpecificationString, lastSundayRegex))
                {
                    dayOfMonthSpecification = new LastWeekdayOfMonthSpecification(DayOfWeek.Sunday);
                    return true;
                }

                string msg = String.Format("Unable to parse last weekday of month specification expression: {0}",
                    dayOfMonthSpecificationString);
                throw new Exception(msg);
            }
            dayOfMonthSpecification = null;
            return false;
        }
    }
}