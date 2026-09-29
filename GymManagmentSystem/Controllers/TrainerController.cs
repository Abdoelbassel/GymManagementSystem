using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.APi.Controllers
{
    [Authorize]

    public class TrainerController : Controller
    {
        private readonly ITrainerServices _trainerServices;

        public TrainerController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }
        public async Task<IActionResult> Index()
        {
            var trainr = await _trainerServices.GetALlTrainerAsync();
            return View(trainr);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model , CancellationToken cancellationToken =default)
        {
            if(!ModelState.IsValid) return View(model);
            var trainer = await _trainerServices.CreateTrainerAsync(model , cancellationToken);

            if (trainer)
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Create";

            return RedirectToAction(nameof(Index));
        } 

        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var trainer = await _trainerServices.GetTrainerDetailsById(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpGet]

        public async Task<IActionResult> Edit (int id , CancellationToken cancellationToken = default)
        {
            var result = await _trainerServices.GetTrainerToUpdate(id );
            if(result is null)
            {
                TempData["ErorrMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id ,CreateTrainerViewModel model , CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return View(model);
            var trainer = await _trainerServices.UpdateTrainerAsync(id, model);
            if (trainer)
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Update";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken = default)
        {
            var result = await _trainerServices.GetTrainerDetailsById(id);
            if(result is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken cancellationToken = default)
        {
            var trainer = await _trainerServices.DeleteTrainerAsync(id);
            if (trainer)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Delete";

            return RedirectToAction(nameof(Index));
        }


    }
}
