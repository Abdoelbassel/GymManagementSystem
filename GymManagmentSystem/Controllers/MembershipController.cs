using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace GymManagmentSystem.APi.Controllers
{
    [Authorize]

    public class MembershipController : Controller
    {

        private readonly IMembershipServices _membershipServices;

        public MembershipController(IMembershipServices membershipServices)
        {
            _membershipServices = membershipServices;
        }
        public async Task<IActionResult> Index()
        {
            var membership = await _membershipServices.GetMembershipsAsync();
            return View(membership);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownListAsync();
                return View(model);
            }

            var result = await _membershipServices.CreateMembershipAsync(model);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
                await PopulateDropDownListAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;
            await PopulateDropDownListAsync();
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Cancel(int id, CancellationToken ct = default)
        {
            var result = await _membershipServices.DeleteMembershipAsync(id);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["SuccessMessage"] = "Membership Cancelled Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
    
        
        private async Task PopulateDropDownListAsync()
        {
            var members = await _membershipServices.GetMemberForDownListAsync();
            var plans = await _membershipServices.GetPlanForDownListAsync();

            ViewBag.members = new SelectList(members, "Id", "Name");
            ViewBag.plans = new SelectList(plans, "Id", "Name");
        }
    }
 } 
