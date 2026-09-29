using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.MemberShipViewModels
{
    public class MembershipViewModel
    {
        public int PlanId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;
        public string PlanName { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
