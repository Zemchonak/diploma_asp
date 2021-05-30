using System;
using System.Net.Http;
using System.Threading.Tasks;
using FitnessCenterManagement.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
using FitnessCenterManagement.WebApp.Helpers;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class LocalizationController : Controller
    {
        private readonly IApiHttpClient _api;

        public LocalizationController(IApiHttpClient api)
        {
            _api = api;
        }

        [Authorize]
        [HttpGet("Index")]
        public async Task<ActionResult> CheckCulture()
        {
            var cultureResponse = await _api.GetUserCulture();
            var userDbculture = (await JsonHelper.DeserializeContentAsync<LanguageModel>(cultureResponse)).Code;
            var cookieCulture = HttpContext.Request.Cookies["Language"];

            if (cookieCulture is null || cookieCulture != userDbculture)
            {
                HttpContext.Response.Cookies.Append("Language", userDbculture);
                Response.Cookies.Append("Language", userDbculture);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string language)
        {
            var response = await _api.PutLocalization(
                JsonHelper.ObjectToStringContent(new ChangeLanguageModel { LanguageCode = language }));

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Response.Cookies.Append("Language", language, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMonths(1),
                });
            }

            return RedirectToAction("Index", "Profile");
        }
    }
}
