using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Interfaces
{
    public interface ISessionRepo : IGenaricRepositorities<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsAsync(
            Expression<Func<Session, bool>>? expression = null,
            CancellationToken ct = default);

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);

        public Task<Session> GetSessionByIdAsync(int id, CancellationToken ct = default);
    }

}
