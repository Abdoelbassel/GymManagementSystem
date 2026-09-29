using GymManagment.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GymManagment.DAL.Models
{
    public class GymUser : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public Gender Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public Address Address { get; set; } = default!;
    }
}

[Owned]
public class Address
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public int BuildingNumber { get; set; }
}
