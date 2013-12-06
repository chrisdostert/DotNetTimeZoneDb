using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class TzDbTimeExpression
    {
        #region Ctors

        private const String zeroRegex = @"-|0";
        private const String hoursRegex = @"\d{1,2}";
        private const String minutesRegex = @"\d{2}";
        private const String secondsRegex = @"\d{2}";
        private const String timeTypeRegex = @"[wsugz]";

        #endregion

        #region Methods

        public static AbstractTime Parse(Queue<Token> unProcessedTokens)
        {
            AbstractTime timeSpecification;
            if (!TryParse(unProcessedTokens, out timeSpecification))
            {
                string msg = String.Format("Unable to parse {0}: {1}", typeof (TzDbTimeExpression).Name,
                    unProcessedTokens.Peek().Value);
                throw new Exception(msg);
            }

            return timeSpecification;
        }

        public static bool TryParse(Queue<Token> unProcessedTokens, out AbstractTime time)
        {
            string timeExpressionRegex = String.Format(@"^(({0})|({1})|({1}:{2})|({1}:{2}:{3})){4}?$", zeroRegex,
                hoursRegex, minutesRegex, secondsRegex, timeTypeRegex);

            if (Regex.IsMatch(unProcessedTokens.Peek().Value, timeExpressionRegex))
            {
                string timeString = unProcessedTokens.Dequeue().Value;
                try
                {
                    if (timeString.EndsWith("w"))
                    {
                        timeString = timeString.TrimEnd('w');
                        time = new LocalTime(ParseTimeString(timeString));
                        return true;
                    }
                    if (timeString.EndsWith("s"))
                    {
                        timeString = timeString.TrimEnd('s');
                        time = new StandardTime(ParseTimeString(timeString));
                        return true;
                    }
                    if (timeString.EndsWith("u") || timeString.EndsWith("g") ||
                        timeString.EndsWith("z"))
                    {
                        timeString = timeString.TrimEnd('u', 'g', 'z');
                        time = new UtcTime(ParseTimeString(timeString));
                        return true;
                    }

                    // default is local time
                    time = new LocalTime(ParseTimeString(timeString));
                    return true;
                }

                catch (Exception e)
                {
                    string msg = String.Format("Unable to parse {0}: {1}", typeof (TzDbTimeExpression).Name, timeString);
                    throw new Exception(msg, e);
                }
            }
            time = null;
            return false;
        }

        public static TimeSpan ParseTimeString(String timeString)
        {
            var timeParts = new Queue<String>(timeString.Split(':'));
            TimeSpan time = TimeSpan.Zero;

            if (zeroRegex == timeParts.Peek())
            {
                return TimeSpan.Zero;
            }

            time = TimeSpan.FromHours(double.Parse(timeParts.Dequeue()));
            if (timeParts.Any())
            {
                TimeSpan minutes = TimeSpan.FromMinutes(double.Parse(timeParts.Dequeue()));
                time = time.Add(minutes);
            }
            if (timeParts.Any())
            {
                TimeSpan seconds = TimeSpan.FromSeconds(double.Parse(timeParts.Dequeue()));
                time = time.Add(seconds);
            }

            return time;
        }

        #endregion
    }
}