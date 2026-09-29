using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string? Note { get; set; }
        public string BloodType { get; set; } = default!;
        public Member Member { get; set; } = default!;
    }
}
