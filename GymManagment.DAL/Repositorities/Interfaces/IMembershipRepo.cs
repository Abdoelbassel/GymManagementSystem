using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Interfaces
{
    public interface IMembershipRepo : IGenaricRepositorities<Membership>
    {
        public Task<IEnumerable<Membership>> GetAllMembershipsAsync(Expression<Func<Membership, bool>> Predicate, bool Tracking = false, CancellationToken cancellation = default);
         
    }
}
