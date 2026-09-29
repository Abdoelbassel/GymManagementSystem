using GymManagment.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace GymManagment.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialties Specialty { get; set; }

        public ICollection<Session> Session { get; set; } = new List<Session>();
    }
}
