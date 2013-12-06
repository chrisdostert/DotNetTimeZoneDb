using System;
using System.Collections.Generic;
using TimeZoneDb.Entities;

namespace TimeZoneDb.Repositories
{
    /// <summary> 
    ///     Note: Implementations should be threadsafe!
    /// </summary>
    public interface IDaylightSavingsAdjustmentRepository
    {
        #region Methods

        void Add(AbstractDaylightSavingsAdjustment daylightSavingsAdjustment);
        AbstractDaylightSavingsAdjustment GetById(Guid daylightSavingsAdjustmentId);
        IEnumerable<AbstractDaylightSavingsAdjustment> GetAll();
        void RemoveAll();

        #endregion
    }
}