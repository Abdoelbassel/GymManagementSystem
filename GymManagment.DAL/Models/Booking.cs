using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Booking : BaseEntity
    {
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Session Session { get; set; } = default!;

        public int SessionId { get; set; }

        public bool IsAttended { get; set; }

        // BookingDAy => CreatedAt in base entity
    }
}
