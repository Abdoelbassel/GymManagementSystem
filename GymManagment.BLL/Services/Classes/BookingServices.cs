using AutoMapper;
using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.BookingViewModels;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem.BLL.Services.Classes
{
    public class BookingServices : IBookingSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BookingServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CancelBookingAsync(int sessionId, int memberId, CancellationToken cancellationToken = default)
        {
            var sessions = await _unitOfWork._SessionRepo.GetByIdAsync(sessionId);
            if (sessions is null)
                return Result.Fail("Session Not Found", ResultKind.NotFound);

            if (sessions.StartDate <= DateTime.Now)
                return Result.Fail("Session Already Started", ResultKind.ValidationError);

            var bookings = await _unitOfWork._BookingRepo.FirstOrDefaultAsync(s => s.SessionId == sessionId && s.MemberId == memberId, tracking: true);
            if (bookings is null)
                return Result.Fail("Booking Not Found", ResultKind.NotFound);

            _unitOfWork._BookingRepo.RemoveAsync(bookings);
            var result = await _unitOfWork.SaveChangAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Cancel Booking Failed", ResultKind.ValidationError);
        }

        public async Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken cancellationToken = default)
        {
            var sessions = await _unitOfWork._SessionRepo.GetByIdAsync(model.SessionId);
            if (sessions is null) return Result.Fail("Session Not Found", ResultKind.NotFound);

            if (sessions.StartDate <= DateTime.Now)
                return Result.Fail("Session Already Started", ResultKind.ValidationError);

            var hasActiveMembership = await _unitOfWork._MembershipRepo.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now);
            if (!hasActiveMembership) return Result.Fail("Member Does Not Have Active Membership", ResultKind.ValidationError);

            var alredyBooked = await _unitOfWork._BookingRepo.AnyAsync(b => b.MemberId == model.MemberId && b.SessionId == model.SessionId);
            if (alredyBooked) return Result.Fail("Member Already Booked This Session", ResultKind.ValidationError);

            var bookedSlots = await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(model.SessionId);
            if (bookedSlots > sessions.Capacity)
                return Result.Fail("Session Is Full", ResultKind.ValidationError);

            _unitOfWork._BookingRepo.AddAsync(new Booking
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false,
                CreatedAt = DateTime.Now,
            });

            var result = await _unitOfWork.SaveChangAsync(cancellationToken);
            return result > 0 ? Result.Ok() : Result.Fail("Booking Failed", ResultKind.ValidationError);
        }

        public async Task<IEnumerable<GetSessionsViewModel>> GetAllSessionAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork._SessionRepo.GetAllSessionsAsync(s => s.EndDate >= DateTime.Now);
            if (!sessions.Any()) return null;

            var mappedSessions = _mapper.Map<IEnumerable<GetSessionsViewModel>>(sessions);

            foreach (var item in mappedSessions)
            {
                item.AvailableSlots = item.Capacity - await _unitOfWork._SessionRepo.GetCountOfBookedSlotsAsync(item.Id);
            }
            return mappedSessions;
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMemberForUpcomingSessionIdAsync(int sessionid, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork._BookingRepo.GetSessionByIdAsync(sessionid);
            var session = await _unitOfWork._SessionRepo.GetByIdAsync(sessionid);


            return booking.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = b.SessionId,
                BookingDate = b.CreatedAt,
                MemberName = b.Member.Name,
                IsAttended = session?.StartDate > DateTime.Now ? false : b.IsAttended
            }).ToList();
        }

        public async Task<IEnumerable<GetMemberForDownListAsync>> GetMemberFromDropDownList(int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork._BookingRepo.GetSessionByIdAsync(sessionId);

            var BookingMemberId = booking.Select(b => b.MemberId);

            var members = await _unitOfWork.GetRepositorities<Member>().GetAllAsync(m => !BookingMemberId.Contains(m.Id));

            return _mapper.Map<IEnumerable<GetMemberForDownListAsync>>(members);

        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForOnComingSessionIdAsync(int sessionid, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork._BookingRepo.GetSessionByIdAsync(sessionid);

            return booking.Select(b => new MemberForSessionViewModel
            {
                MemberId = b.MemberId,
                SessionId = b.SessionId,
                MemberName = b.Member.Name,
                IsAttended = b.IsAttended,
                BookingDate = b.CreatedAt,
            }).ToList();
        }

        public async Task<Result> MarkAttendedAsync(int memberid, int sessionid, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork._BookingRepo.FirstOrDefaultAsync(b => b.MemberId == memberid && b.SessionId == sessionid, tracking: true);
            if (booking is null)
                return Result.Fail("Booking Not Found", ResultKind.NotFound);

            booking.IsAttended = true;

            _unitOfWork._BookingRepo.UpdatedAsync(booking);
            var result = await _unitOfWork.SaveChangAsync(cancellationToken);

            return result > 0 ? Result.Ok() : Result.Fail("Mark Attended Failed", ResultKind.ValidationError);


        }
    };
}
