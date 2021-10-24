using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        public ReportsController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
