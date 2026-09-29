using System;
using System.Collections.Generic;
using System.Text;
using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using GymManagmentSystem.DAL.Repositorities.Interfaces;


namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface IUnitOfWork
    {
        public IMembershipRepo _MembershipRepo { get; }
        public IBookingRepo _BookingRepo { get; }
        public ISessionRepo _SessionRepo { get; }
        public IGenaricRepositorities<TEntity> GetRepositorities<TEntity>() where TEntity : BaseEntity , new();
        public Task<int> SaveChangAsync(CancellationToken cancellationToken);
    }
}
