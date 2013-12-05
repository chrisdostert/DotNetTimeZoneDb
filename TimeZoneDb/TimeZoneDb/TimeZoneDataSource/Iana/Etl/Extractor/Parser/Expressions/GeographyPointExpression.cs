using System;
using System.Collections.Generic;
using System.Spatial;
using System.Text.RegularExpressions;
using TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Tokenizer;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.Extractor.Parser.Expressions
{
    public class GeographyPointExpression
    {
        public static GeographyPoint Parse(Queue<Token> unProcessedTokens)
        {
            string geographyPointString = unProcessedTokens.Dequeue().Value;
            const String latitudeCaptureGroupKey = "latitude";
            const String longitudeCaptureGroupKey = "longitude";
            const String latitudeRegexPattern = @"([-+]\d{4}|\d{6})";
            const String longitudeRegexPattern = @"([-+]\d{5}|\d{7})";

            string latitudeLongitudeRegex = String.Format(@"(?<{0}>{1})(?<{2}>{3})", latitudeCaptureGroupKey,
                latitudeRegexPattern, longitudeCaptureGroupKey, longitudeRegexPattern);

            Match match = Regex.Match(geographyPointString, latitudeLongitudeRegex);
            if (match.Success)
            {
                double latitude = double.Parse(match.Groups[latitudeCaptureGroupKey].Value);
                double longitude = double.Parse(match.Groups[longitudeCaptureGroupKey].Value);
                return GeographyPoint.Create(latitude, longitude);
            }

            string msg = String.Format("Unable to parse geography point expression: {0}", geographyPointString);
            throw new Exception(msg);
        }
    }
}