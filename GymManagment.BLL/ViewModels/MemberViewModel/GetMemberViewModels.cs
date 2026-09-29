using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.MemberViewModel
{
    public class GetMemberViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Photo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        public string? DataOfBirth { get; set; }
        public string? Address { get; set; }
        public string? PlanName { get; set; }
        public string? MemberShipStartDate { get; set; }
        public string? MemberShipEndDate { get; set; }


    }
}
