using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Classes
{
    public class MembershipRepo : GenaricRepositorities<Membership>, IMembershipRepo
    {
        private readonly GymAppContext _appContext;
        public MembershipRepo(GymAppContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }
        public async Task<IEnumerable<Membership>> GetAllMembershipsAsync(Expression<Func<Membership, bool>> Predicate, bool Tracking = false, CancellationToken cancellation = default)
        {
            IQueryable<Membership> Query = _appContext.Memberships.AsNoTracking().Include(m => m.member).Include(m => m.plan);
            if(Predicate != null)
                Query = Query.Where(Predicate);


            return await Query.ToListAsync();
        }  
    }
}
