using System;
using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.Repositories
{
    /// <summary> 
    ///     Note: Implementations should be threadsafe!
    /// </summary>
    public interface ITimeZoneRepository
    {
        #region Methods

        IEnumerable<DbTimeZone> GetAll();
        DbTimeZone GetByIanaId(String ianaId);
        void Update(DbTimeZone dbTimeZone);
        void Add(DbTimeZone dbTimeZone);
        void RemoveAll();

        #endregion
    }
}