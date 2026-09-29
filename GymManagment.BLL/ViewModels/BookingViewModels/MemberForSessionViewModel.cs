using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.BookingViewModels
{
    public class MemberForSessionViewModel
    {
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;
        public bool IsAttended { get; set; } = false;
        public DateTime BookingDate { get; set; }
    }
}
