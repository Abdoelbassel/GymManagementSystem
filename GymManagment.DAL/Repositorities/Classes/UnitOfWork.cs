using GymManagment.DAL.Data.Context;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.DAL.Repositorities.Classes;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly GymAppContext _gymAppContext;

        public readonly Dictionary<string, object> _repositorities = [];
        public IMembershipRepo _MembershipRepo { get; }
        public ISessionRepo _SessionRepo { get; }

        public IBookingRepo _BookingRepo { get; }

        public UnitOfWork(GymAppContext gymAppContext, ISessionRepo sessionRepo , IMembershipRepo membershipRepo, IBookingRepo bookingRepo)
        {
            _gymAppContext = gymAppContext;
            _MembershipRepo = membershipRepo;
            _SessionRepo = sessionRepo;
            _BookingRepo = bookingRepo;
        }
        public IGenaricRepositorities<TEntity> GetRepositorities<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;

            if (_repositorities.TryGetValue(typeName, out object value))
                return (IGenaricRepositorities<TEntity>)value;

            else
            {
                var repo = new GenaricRepositorities<TEntity>(_gymAppContext);
                _repositorities[typeName] = repo;
                return repo;

            }
        }

        public async Task<int> SaveChangAsync(CancellationToken cancellationToken)
        {
            return await _gymAppContext.SaveChangesAsync(cancellationToken);
        }
    }
}
