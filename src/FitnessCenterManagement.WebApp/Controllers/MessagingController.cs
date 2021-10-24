using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class MessagingController : Controller
    {
        private readonly IApiHttpClient _api;

        public MessagingController(IApiHttpClient api)
        {
            _api = api;
        }

        [Authorize]
        [HttpGet("")]
        public IActionResult Index(int id = 0)
        {
            return View();
        }
    }
}
