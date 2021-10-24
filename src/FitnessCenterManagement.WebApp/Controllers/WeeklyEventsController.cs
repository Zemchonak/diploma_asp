using FitnessCenterManagement.WebApp.Helpers;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class WeeklyEventsController : Controller
    {
        private readonly IApiHttpClient _api;

        public WeeklyEventsController(IApiHttpClient api)
        {
            _api = api;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetWeeklyEvents();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<WeeklyEventModel>>(response);

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("create")]
        public IActionResult Create()
        {
            var model = new WeeklyEventModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("findByDay/{dayNo}")]
        public async Task<IActionResult> Create([FromRoute] int dayNo)
        {
            if (dayNo > 7 || dayNo < 1)
            {
                return NotFound();
            }

            var response = await _api.GetWeeklyEvents();
            var weeklyEvents = await JsonHelper.DeserializeContentAsync<List<WeeklyEventModel>>(response);
            for (int i = 0; i < weeklyEvents.Count; i++)
            {
                var fitnessEvent = await JsonHelper.DeserializeContentAsync<FitnessEventModel>(
                    await _api.GetFitnessEvents(weeklyEvents[i].FitnessEventId));
                var service = await JsonHelper.DeserializeContentAsync<ServiceModel>(await _api.GetServices(fitnessEvent.ServiceId));

                weeklyEvents[i].FitnessEventInfo = $"{service.Name} - {fitnessEvent.VenueInfo} ({fitnessEvent.Minutes} мин)";
                var venue = await JsonHelper.DeserializeContentAsync<VenueModel>(
                    await _api.GetVenues(fitnessEvent.VenueId));
                weeklyEvents[i].Location = $"{venue.Name}";
            }

            return new JsonResult(weeklyEvents.Where(e => e.DayOfWeek == dayNo).ToList());
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(WeeklyEventModel model)
        {
            var response = await _api.PostWeeklyEvents(JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var response = await _api.GetWeeklyEvents(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<WeeklyEventModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("edit")]
        public async Task<IActionResult> Update(WeeklyEventModel model)
        {
            var response = await _api.PutWeeklyEvents(model.Id, JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var response = await _api.GetWeeklyEvents(id);
            var model = await JsonHelper.DeserializeContentAsync<WeeklyEventModel>(response);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteWeeklyEvents(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var responseContent = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ViewBag.ErrorMessage = responseContent.ErrorMessage;
            return RedirectToAction("Remove", new { id });
        }
    }
}
