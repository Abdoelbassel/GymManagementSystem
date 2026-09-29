using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Membership : BaseEntity
    {
        public Member member { get; set; } = default!;

        public int MemberId { get; set; }

        public PLan plan { get; set; } = default!;
        public int PlanId { get; set; }

        public DateTime EndDate { get; set; }

        //[NotMapped]
        //public bool IsActive() ⇒ EndDate > DateOnly.FromDateTime(DateTime.Now);
        [NotMapped]
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";

        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;
    }
}
