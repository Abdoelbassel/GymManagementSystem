using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.ViewModels.BookingViewModels;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Interfaces
{
    public interface IBookingSevices
    {
        public Task<IEnumerable<GetSessionsViewModel>> GetAllSessionAsync(CancellationToken ct = default);
        public Task<IEnumerable<MemberForSessionViewModel>> GetMemberForUpcomingSessionIdAsync(int sessionid , CancellationToken cancellationToken = default);
        public Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOnComingSessionIdAsync(int sessionid , CancellationToken cancellationToken = default);
        public Task<Result> CreateBookingAsync(CreateBookingViewModel viewModel, CancellationToken cancellationToken = default);
        public Task<Result> CancelBookingAsync(int sessionid, int memberid, CancellationToken cancellationToken = default);
        public Task<Result> MarkAttendedAsync(int memberid, int sessionid, CancellationToken cancellationToken = default);
        public Task<IEnumerable<GetMemberForDownListAsync>> GetMemberFromDropDownList(int sessionId, CancellationToken ct = default);

    }
}
