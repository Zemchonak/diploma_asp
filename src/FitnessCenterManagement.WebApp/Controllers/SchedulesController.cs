using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class SchedulesController : Controller
    {
        private readonly IApiHttpClient _api;

        public SchedulesController(IApiHttpClient api)
        {
            _api = api;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
