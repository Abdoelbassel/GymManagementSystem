using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Interfaces
{
    public interface IGenaricRepositorities<TEntity> where TEntity : BaseEntity
    {
        // GetALlAsync
        // GetById
        // AddAsync
        // UpdatedAsync
        // RemoveAsync

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null,bool Tracking = false , CancellationToken cancellation = default);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public void AddAsync(
                    TEntity entity,
                    CancellationToken cancellationToken = default);
        public void RemoveAsync(TEntity entity, CancellationToken ct = default);
        public void UpdatedAsync(TEntity entity, CancellationToken ct = default);
        public Task<bool> AnyAsync(Expression<Func<TEntity , bool>> expression , CancellationToken cancellationToken = default);
        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = false, CancellationToken cancellation = default);

        public Task<int> CountAsync (Expression<Func<TEntity , bool>>? expression = null, CancellationToken ct = default);
    }
}
