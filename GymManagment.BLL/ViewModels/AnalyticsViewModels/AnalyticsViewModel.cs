using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.ViewModels.AnalyticsViewModels
{
    public class AnalyticsViewModel
    {
            public int TotalMembers { get; set; }
            public int ActiveMembers { get; set; }
            public int TotalTrainers { get; set; }
            public int UpcomingSessions { get; set; }
            public int OngoingSessions { get; set; }
            public int CompletedSessions { get; set; }
    }
}
