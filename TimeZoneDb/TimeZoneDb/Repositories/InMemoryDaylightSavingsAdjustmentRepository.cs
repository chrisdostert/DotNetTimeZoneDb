using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.Repositories
{
    public class InMemoryDaylightSavingsAdjustmentRepository : IDaylightSavingsAdjustmentRepository
    {
        #region Ctors

        public InMemoryDaylightSavingsAdjustmentRepository()
        {
            _daylightSavingsAdjustmentDatabase = new ConcurrentDictionary<Guid, AbstractDaylightSavingsAdjustment>();
        }

        #endregion

        #region Fields

        private readonly ConcurrentDictionary<Guid, AbstractDaylightSavingsAdjustment>
            _daylightSavingsAdjustmentDatabase;

        #endregion

        #region Methods

        public void Add(AbstractDaylightSavingsAdjustment daylightSavingsAdjustment)
        {
            _daylightSavingsAdjustmentDatabase.TryAdd(daylightSavingsAdjustment.Id, daylightSavingsAdjustment);
        }

        public AbstractDaylightSavingsAdjustment GetById(Guid daylightSavingsAdjustmentId)
        {
            return _daylightSavingsAdjustmentDatabase[daylightSavingsAdjustmentId];
        }

        public IEnumerable<AbstractDaylightSavingsAdjustment> GetAll()
        {
            return _daylightSavingsAdjustmentDatabase.Values;
        }

        public void RemoveAll()
        {
            _daylightSavingsAdjustmentDatabase.Clear();
        }

        #endregion
    }
}