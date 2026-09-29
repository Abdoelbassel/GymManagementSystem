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
    public class BookingRepo : GenaricRepositorities<Booking> , IBookingRepo
    {
        private readonly GymAppContext _context;

        public BookingRepo(GymAppContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetSessionByIdAsync(int sessionid, CancellationTokenSource ct = default)
        {
            return await _context.Bookings.AsNoTracking()
                                                        .Include(b => b.Member)
                                                        .Where(b => b.SessionId == sessionid)
                                                        .ToListAsync();
        }
    }
}
