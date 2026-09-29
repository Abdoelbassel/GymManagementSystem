using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentSystem.APi.Controllers
{
    [Authorize]

    public class SessionController : Controller
    {
        private readonly ISessionServices _sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            _sessionServices = sessionServices;
        }
        public async Task<IActionResult> Index()
        {
            var sessions = await _sessionServices.GetSessionsAsync();
            return View(sessions.value);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownListAsync();
                return View(model);
            }

            var result = await _sessionServices.CreateSessionAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;

            await PopulateDropDownListAsync();
            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var result = await _sessionServices.GetSessionById(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            return View(result.value);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken st = default)
        {
            var sessions = await _sessionServices.GetToUpdateSessionAsync(id, st);
            if (!sessions.success)
            {
                TempData["ErrorMessage"] = sessions.error;
                return RedirectToAction(nameof(Index));
            }

            var trainers = await _sessionServices.GetTrainersFroDropDownList();
            ViewBag.Trainers = new SelectList(trainers.value, "Id", "Name");

            return View(sessions.value);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdateSessionViewModel model, CancellationToken ctr = default)
        {
            if (!ModelState.IsValid)
            {
                var trainers = await _sessionServices.GetTrainersFroDropDownList();
                ViewBag.Trainers = new SelectList(trainers.value, "Id", "Name");

                return View(model);
            }

            var result = await _sessionServices.UpdateToSessionAsync(id, model, ctr);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                var trainers = await _sessionServices.GetTrainersFroDropDownList();
                ViewBag.Trainers = new SelectList(trainers.value, "Id", "Name");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var result = await _sessionServices.GetSessionById(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            return View(result.value);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            var result = await _sessionServices.DeleteSessionAsync(id, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDownListAsync()
        {
            var trainers = await _sessionServices.GetTrainersFroDropDownList();
            var categories = await _sessionServices.GetCategoriesFroDropDownList();

            ViewBag.Trainers = new SelectList(trainers.value, "Id", "Name");
            ViewBag.Categories = new SelectList(categories.value, "Id", "CategoryName");
        }

    }
}
