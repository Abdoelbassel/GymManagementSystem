using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Classes
{
    public class GenaricRepositorities<TEntity>
    : IGenaricRepositorities<TEntity>
    where TEntity : BaseEntity
    {
        private readonly GymAppContext _appContext;
        private readonly DbSet<TEntity> _set;

        public GenaricRepositorities(GymAppContext appContext)
        {
            _appContext = appContext;
            _set = _appContext.Set<TEntity>();
        }

        public void AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            _set.AddAsync(entity, cancellationToken);

        }

        public void RemoveAsync(TEntity entity , CancellationToken ct = default)
        {
            _set.Remove(entity);
        }

        public void UpdatedAsync(TEntity entity , CancellationToken ct = default)
        {
            _set.Update(entity);
        }

        public Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return _set
                .AsNoTracking()
                .AnyAsync(expression, cancellationToken);
        }

        public Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            bool tracking = false,
            CancellationToken cancellationToken = default)
        {
            var query = tracking ? _set : _set.AsNoTracking();

            return query.FirstOrDefaultAsync(
                expression,
                cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? expression = null,
            bool tracking = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query =
                tracking ? _set : _set.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _set.FindAsync(
                [id],
                cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken ct = default)
        {
            if(expression == null)
                return _set.AsNoTracking().CountAsync(ct);
            else
                return _set.AsNoTracking().CountAsync(expression, ct);
        }
    }
}
