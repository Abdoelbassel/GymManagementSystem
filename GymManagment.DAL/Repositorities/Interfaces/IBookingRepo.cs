using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.DAL.Repositorities.Interfaces
{
    public interface IBookingRepo : IGenaricRepositorities<Booking>
    {
        public Task<IEnumerable<Booking>> GetSessionByIdAsync(int sessionid , CancellationTokenSource ct = default);

    }
}
