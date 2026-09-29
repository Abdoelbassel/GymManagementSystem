using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        //public ICollection<Category> Categories { get; set; } = new List<Category>();

        public Category Category { get; set; } = default!;
        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
