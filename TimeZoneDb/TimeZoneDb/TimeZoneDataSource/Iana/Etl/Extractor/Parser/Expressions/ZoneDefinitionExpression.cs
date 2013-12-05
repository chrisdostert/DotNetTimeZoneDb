using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class ZoneDefinitionExpression
    {
        #region Methods

        public static bool TryParse(Queue<Token> unProcessedTokens, out TzDbZoneDefinition tzDbZoneDefinition)
        {
            try
            {
                // the value of the first token of every zone entry is Zone
                TzDbZone? tzDbZone;
                if (ZoneExpression.TryParse(unProcessedTokens, out tzDbZone))
                {
                    // if Until column of zone line is null this zone has no continuation lines
                    if (null == tzDbZone.Value.Until)
                    {
                        tzDbZoneDefinition = new TzDbZoneDefinition(tzDbZone.Value, new List<TzDbZoneContinuation>());

                        return true;
                    }

                    // dequeue start of line token and discard
                    unProcessedTokens.Dequeue();

                    var continuationLines = new List<TzDbZoneContinuation>
                    {
                        ZoneContinuationExpression.Parse(unProcessedTokens, tzDbZone.Value.Name)
                    };

                    // keep parsing zone continuations until we hit one that has a null until column
                    while (null != continuationLines.Last().Until)
                    {
                        // dequeue start of line token and discard
                        unProcessedTokens.Dequeue();

                        continuationLines.Add(ZoneContinuationExpression.Parse(unProcessedTokens, tzDbZone.Value.Name));
                    }

                    tzDbZoneDefinition = new TzDbZoneDefinition(tzDbZone.Value, continuationLines);
                    return true;
                }
                tzDbZoneDefinition = null;
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured while parsing zone definition", e);
            }
        }

        #endregion
    }
}