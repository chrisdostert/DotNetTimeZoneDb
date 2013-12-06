using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.Repositories
{
    public class InMemoryTimeZoneRepository : ITimeZoneRepository
    {
        #region Ctors

        public InMemoryTimeZoneRepository()
        {
            _timeZoneDatabase = new ConcurrentDictionary<string, DbTimeZone>();
        }

        #endregion

        #region Fields

        private readonly ConcurrentDictionary<String, DbTimeZone> _timeZoneDatabase;

        #endregion

        #region Methods

        public void Add(DbTimeZone dbTimeZone)
        {
            _timeZoneDatabase.TryAdd(dbTimeZone.IanaId, dbTimeZone);
        }

        public void Update(DbTimeZone dbTimeZone)
        {
            _timeZoneDatabase[dbTimeZone.IanaId] = dbTimeZone;
        }

        public DbTimeZone GetByIanaId(String ianaId)
        {
            return _timeZoneDatabase[ianaId];
        }

        public IEnumerable<DbTimeZone> GetAll()
        {
            return _timeZoneDatabase.Values;
        }

        public void RemoveAll()
        {
            _timeZoneDatabase.Clear();
        }

        #endregion
    }
}