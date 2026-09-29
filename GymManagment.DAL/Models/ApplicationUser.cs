using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { set; get; } = default!;
        public string LastName { set; get; } = default!;
    }
}
