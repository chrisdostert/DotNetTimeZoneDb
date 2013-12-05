using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class MonthOfYearExpression
    {
        #region Methods

        public static MonthOfYear Parse(Queue<Token> unProcessedTokens)
        {
            MonthOfYear? monthOfYear;
            if (!TryParse(unProcessedTokens, out monthOfYear))
            {
                string msg = String.Format("Unable to parse {0} : {1}", typeof (MonthOfYearExpression).Name,
                    unProcessedTokens.Peek().Value);
                throw new Exception(msg);
            }

            return monthOfYear.Value;
        }

        public static bool TryParse(Queue<Token> unProcessedTokens, out MonthOfYear? monthOfYear)
        {
            const String januaryRegex = @"(January)|(Jan)";
            const String februaryRegex = @"(February)|(Feb)";
            const String marchRegex = @"(March)|(Mar)";
            const String aprilRegex = @"(April)|(Apr)";
            const String mayRegex = @"May";
            const String juneRegex = @"(June)|(Jun)";
            const String julyRegex = @"(July)|(Jul)";
            const String augustRegex = @"(August)|(Aug)";
            const String septemberRegex = @"(September)|(Sep)";
            const String octoberRegex = @"(October)|(Oct)";
            const String novemberRegex = @"(November)|(Nov)";
            const String decemberRegex = @"(December)|(Dec)";

            string monthOfYearExpressionRegex = String.Format("^({0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11})$",
                januaryRegex, februaryRegex, marchRegex, aprilRegex, mayRegex, juneRegex, julyRegex, augustRegex,
                septemberRegex, octoberRegex, novemberRegex, decemberRegex);

            if (Regex.IsMatch(unProcessedTokens.Peek().Value, monthOfYearExpressionRegex))
            {
                string monthOfYearString = unProcessedTokens.Dequeue().Value;

                if (Regex.IsMatch(monthOfYearString, januaryRegex))
                {
                    monthOfYear = MonthOfYear.January;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, februaryRegex))
                {
                    monthOfYear = MonthOfYear.February;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, marchRegex))
                {
                    monthOfYear = MonthOfYear.March;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, aprilRegex))
                {
                    monthOfYear = MonthOfYear.April;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, mayRegex))
                {
                    monthOfYear = MonthOfYear.May;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, juneRegex))
                {
                    monthOfYear = MonthOfYear.June;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, julyRegex))
                {
                    monthOfYear = MonthOfYear.July;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, augustRegex))
                {
                    monthOfYear = MonthOfYear.August;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, septemberRegex))
                {
                    monthOfYear = MonthOfYear.September;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, octoberRegex))
                {
                    monthOfYear = MonthOfYear.October;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, novemberRegex))
                {
                    monthOfYear = MonthOfYear.November;
                    return true;
                }
                if (Regex.IsMatch(monthOfYearString, decemberRegex))
                {
                    monthOfYear = MonthOfYear.December;
                    return true;
                }

                string msg = String.Format("Unable to parse month of year expression: {0}", monthOfYearString);
                throw new Exception(msg);
            }

            monthOfYear = null;
            return false;
        }

        #endregion
    }
}