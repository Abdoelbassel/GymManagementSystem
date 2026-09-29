using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.BookingViewModels;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using GymManagmentSystem.DAL.Repositorities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace GymManagmentSystem.APi.Controllers
{
    [Authorize]

    public class BookingsController : Controller
    {
        private readonly IBookingSevices _bookingSevices;

        public BookingsController(IBookingSevices bookingSevices)
        {
            _bookingSevices = bookingSevices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await _bookingSevices.GetAllSessionAsync(ct);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions( int id, CancellationToken ct = default)
        {
            var result = await _bookingSevices.GetMembersForOnComingSessionIdAsync(sessionid: id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession (int id, CancellationToken ct = default)
        {
            var result = await _bookingSevices.GetMemberForUpcomingSessionIdAsync(sessionid: id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create (int id , CancellationToken ct  = default)
        {
            var members = await GetMemberForDropDown(id , ct);
            ViewBag.Members = new SelectList(members ,"Id", "Name");
            ViewBag.Session = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (CreateBookingViewModel model , CancellationToken ct = default)
        {
            var booking = await _bookingSevices.CreateBookingAsync(model , ct);

            TempData[booking.success ? "SuccessMessage" : "ErrorMessage"] =
                 booking.success ? "Booking created successfully." : booking.error;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken cancellationToken)
        {
            var result = await _bookingSevices.MarkAttendedAsync(memberId, sessionId, cancellationToken);

            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success ? "Marked attended successfully." : result.error;

            return RedirectToAction(actionName: nameof(GetMembersForOngoingSessions), routeValues: new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken cancellationToken)
        {
            var result = await _bookingSevices.CancelBookingAsync(memberId, sessionId, cancellationToken);

            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success ? "Booking canceled successfully." : result.error;

            return RedirectToAction(actionName: nameof(GetMembersForUpcomingSession), routeValues: new { id = sessionId });
        }



        private  Task<IEnumerable<GetMemberForDownListAsync>> GetMemberForDropDown(int sessionId, CancellationToken ct)
        {
            var members =  _bookingSevices.GetMemberFromDropDownList(sessionId);
            return members;
        }

    }
}
