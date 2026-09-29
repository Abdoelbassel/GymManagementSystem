using GymManagment.DAL.Models;
using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace GymManagmentSystem.Pl.Controllers
{
    [Authorize]

    public class PlanController : Controller
    {
        private readonly IPlanServices _planRepo;

        public PlanController(IPlanServices PlanRepo)
        {
            _planRepo = PlanRepo;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _planRepo.GetAllPlansAsync();
            return View(plans.value);
        }

        public  async Task<IActionResult> Details(int id)
        {
            var Plan = await _planRepo.GetPlanByIdAsync(id);
            if (!Plan.success)
            {
                return RedirectToAction("Index");
            }
            return View(Plan.value);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct)
        {
            var plan = await _planRepo.GetPlanToUpdateAsync(id,ct);
            if (!plan.success)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan.value);
        }

        [HttpPost]

        public async Task<IActionResult> Edit (int id , UpdateToPlanViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);

            var result = await _planRepo.UpdatePlanAsync(id,model,ct);
            if (result.success)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Plan Failed To Updated";

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await _planRepo.ToggleActivationAsync(id, ct);
            if (result.success)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed to Toggle Plan Status";
            return RedirectToAction(nameof(Index));

        }
    }
}