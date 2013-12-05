using System;
using System.Collections.Generic;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;
using TimeZoneDb.ValueObjects;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class TzDbDateTimeExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out TzDbAbstractDateTime dateTime)
        {
            try
            {
                Date date;
                if (TzDbDateExpression.TryParse(unProcessedTokens, out date))
                {
                    AbstractTime time;
                    if (!TzDbTimeExpression.TryParse(unProcessedTokens, out time))
                    {
                        // default to specify local time at midnight
                        time = new LocalTime();
                    }

                    dateTime = CreateDateTime(date, time);
                    return true;
                }

                dateTime = null;
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured while parsing date time specification expression", e);
            }
        }

        /// <summary>
        ///     Polymorphic factory for creating a DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static TzDbAbstractDateTime CreateDateTime(Date date, AbstractTime time)
        {
            var localTime = time as LocalTime;
            if (null != localTime)
            {
                return new TzDbLocalDateTime(date, localTime);
            }
            var standardTime = time as StandardTime;
            if (null != standardTime)
            {
                return new TzDbStandardDateTime(date, standardTime);
            }
            var utcTime = time as UtcTime;
            if (null != utcTime)
            {
                return new TzDbUtcDateTime(date, utcTime);
            }

            string msg = String.Format("Received unsupported implementation of {0}: {1}",
                typeof (AbstractTime).Name, time.GetType().Name);
            throw new Exception(msg);
        }

        #endregion
    }
}