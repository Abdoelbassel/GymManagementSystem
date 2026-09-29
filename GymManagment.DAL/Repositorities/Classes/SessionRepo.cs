using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static System.Net.WebRequestMethods;

namespace GymManagmentSystem.DAL.Repositorities.Classes
{
    public class SessionRepo : GenaricRepositorities<Session>, ISessionRepo
    {
        private readonly GymAppContext appContext;

        public SessionRepo(GymAppContext appContext) : base(appContext)
        {
            this.appContext = appContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync(Expression<Func<Session, bool>>? expression,CancellationToken cancellationToken =default)
        {
            IQueryable<Session> query = appContext.Sessions
                    .AsNoTracking()
                    .Include(s => s.Trainer)
                    .Include(s => s.Category); 

            if (expression != null)
            {
                query = query.Where(expression);
            }
            return await query.ToListAsync();
        }

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct)
        {
            return appContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, ct);
        }

        public async Task<Session?> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            return await appContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
