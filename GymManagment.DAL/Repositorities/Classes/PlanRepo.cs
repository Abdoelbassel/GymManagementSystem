using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Classes
{
    public class PlanRepo : IPlanRepo
    {
        private readonly GymAppContext _appContext;

        public PlanRepo(GymAppContext gymAppContext)
        {
            _appContext = gymAppContext;
        }
        public Task AddAsync(PLan plan, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PLan>> GetAllAsync(bool Tracking = false, CancellationToken cancellation = default)
        {
            IQueryable<PLan> Query = Tracking ? _appContext.Plans : _appContext.Plans.AsNoTracking();
            return await Query.ToListAsync(cancellation);
        }

        public Task GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(PLan entity, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdatedAsync(PLan entity, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

     
    }
}
