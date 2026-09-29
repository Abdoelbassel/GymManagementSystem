using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace GymManagmentSystem.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {

        //private readonly ILogger _logger;
        private readonly IAnalyticsServices _analyticsServices;

        public HomeController(IAnalyticsServices analyticsServices)
        {
            //_logger = logger;
            _analyticsServices = analyticsServices;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _analyticsServices.GetAnalyticsAsync();
            return View(data);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
