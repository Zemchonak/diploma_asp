using FitnessCenterManagement.WebApp.Helpers;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
using FitnessCenterManagement.WebApp.Views.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Authorize(Roles = Constants.ManagerRole)]
    [Route("[controller]")]
    public class FitnessEventsController : Controller
    {
        private readonly IApiHttpClient _api;

        public FitnessEventsController(IApiHttpClient api)
        {
            _api = api;
        }

        [AllowAnonymous]
        [HttpGet("items")]
        public async Task<IActionResult> GetAllFitnessEvents()
        {
            var response = await _api.GetFitnessEvents();

            var content = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<FitnessEventModel>>(response);

            var model = new List<FitnessEventModel>();

            foreach (var one in content)
            {
                var venueResponse = await _api.GetVenues(one.VenueId);
                var venue = await JsonHelper.DeserializeContentAsync<VenueModel>(venueResponse);
                var serviceResponse = await _api.GetServices(one.ServiceId);
                var service = await JsonHelper.DeserializeContentAsync<ServiceModel>(serviceResponse);

                model.Add(new FitnessEventModel
                {
                    Id = one.Id,
                    Minutes = one.Minutes,
                    VenueId = one.VenueId,
                    ServiceId = one.ServiceId,
                    VenueInfo = $"{venue.Name} ({venue.Location})",
                    ServiceInfo = $"{service.Name} ({SharedStringRes.CurrencySymbol}{service.Price})",
                });
            }

            return new JsonResult(model.AsReadOnly());
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetFitnessEvents();

            var content = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<FitnessEventModel>>(response);

            var model = new List<FitnessEventModel>();

            foreach(var one in content)
            {
                var venueResponse = await _api.GetVenues(one.VenueId);
                var venue = await JsonHelper.DeserializeContentAsync<VenueModel>(venueResponse);
                var serviceResponse = await _api.GetServices(one.ServiceId);
                var service = await JsonHelper.DeserializeContentAsync<ServiceModel>(serviceResponse);

                model.Add(new FitnessEventModel
                {
                    Id = one.Id,
                    Minutes = one.Minutes,
                    VenueId = one.VenueId,
                    ServiceId = one.ServiceId,
                    VenueInfo = $"{venue.Name} ({venue.Location})",
                    ServiceInfo = $"{service.Name} ({SharedStringRes.CurrencySymbol}{service.Price})",
                });
            }

            return View(model.AsReadOnly());
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(FitnessEventModel model)
        {
            var response = await _api.PostFitnessEvents(JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var response = await _api.GetFitnessEvents(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<FitnessEventModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Update(FitnessEventModel model)
        {
            var response = await _api.PutFitnessEvents(model.Id, JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var response = await _api.GetFitnessEvents(id);
            var model = await JsonHelper.DeserializeContentAsync<FitnessEventModel>(response);
            var service = await JsonHelper.DeserializeContentAsync<ServiceModel>(await _api.GetServices(model.ServiceId));
            model.ServiceInfo = $"{service.Name} ({SharedStringRes.CurrencySymbol}{service.Price})"; 
            var venue = (await JsonHelper.DeserializeContentAsync<VenueModel>(await _api.GetVenues(model.VenueId)));
            model.VenueInfo = $"{venue.Name} ({venue.Location})";
            return View(model);
        }

        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteFitnessEvents(id);
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
