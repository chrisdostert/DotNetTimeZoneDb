using System;
using TimeZoneDb.Entities;
using TimeZoneDb.ValueObjects;
using TimeZoneDb.ValueObjects.Specifications;

namespace TimeZoneDb.TimeZoneDataSource.Iana.Etl.TzDbTypes
{
    public struct TzDbRule
    {
        #region Ctors

        public TzDbRule(String name, AbstractYearSpecification from, AbstractYearSpecification to, String type,
            MonthOfYear @in, AbstractDayOfMonthSpecification on, AbstractTime at, TimeSpan save,
            TimeZoneAbbreviationVariable letter)
            : this()
        {
            Name = name;
            From = from;
            To = to;
            Type = type;
            In = @in;
            On = on;
            At = at;
            Save = save;
            Letter = letter;
        }

        #endregion

        #region Properties

        public String Name { get; private set; }
        public AbstractYearSpecification From { get; private set; }
        public AbstractYearSpecification To { get; private set; }
        public String Type { get; private set; }
        public MonthOfYear In { get; private set; }
        public AbstractDayOfMonthSpecification On { get; private set; }
        public AbstractTime At { get; private set; }
        public TimeSpan Save { get; private set; }
        public TimeZoneAbbreviationVariable Letter { get; private set; }

        #endregion

        #region Conversion Methods

        public DaylightSavingsRule ToOffsetAdjustmentSpecification()
        {
            return new DaylightSavingsRule(Save, From, To, In, On, At, Letter);
        }

        #endregion
    }
}