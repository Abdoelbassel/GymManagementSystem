using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class  Member: GymUser
    {
        public string? Photo { get; set; }

        // DataOfBirth => JoinDate
        public DateOnly JoinDate { get; set; }
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

        public ICollection<Booking> Booking { get; set; } = new List<Booking>();
    }
}
