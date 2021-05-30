using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
using FitnessCenterManagement.WebApp.Helpers;

namespace FitnessCenterManagement.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiHttpClient _api;

        public HomeController(IApiHttpClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _api.Ping();
                return response.IsSuccessStatusCode ? View() : RedirectToAction("NoApiConnection");
            }
            catch
            {
                return RedirectToAction("NoApiConnection");
            }
        }

        public IActionResult NoApiConnection()
        {
            return View();
        }

        public IActionResult CarouselPartial()
        {
            return PartialView("_MainCarousel");
        }
    }
}