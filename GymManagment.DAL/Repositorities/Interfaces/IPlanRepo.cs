using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Interfaces
{
    public interface IPlanRepo
    {
        public Task<IEnumerable<PLan>> GetAllAsync(bool Tracking = false, CancellationToken cancellation = default);
        public Task GetByIdAsync(int id, CancellationToken ct = default);
        public Task AddAsync(PLan plan, CancellationToken cancellation = default);
        public Task RemoveAsync(PLan entity, CancellationToken cancellation = default);
        public Task UpdatedAsync(PLan entity, CancellationToken cancellation = default);
    }
}
