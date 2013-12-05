using System;

namespace TimeZoneDb.ValueObjects
{
    public struct TimeZoneAlias
    {
        #region Ctors

        public TimeZoneAlias(String name, Guid? id = null) : this()
        {
            Name = name;
            Id = id ?? Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }
        public String Name { get; private set; }

        #endregion
    }
}