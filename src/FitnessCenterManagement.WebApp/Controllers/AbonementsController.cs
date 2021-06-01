using FitnessCenterManagement.WebApp.Helpers;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
using FitnessCenterManagement.WebApp.Views.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class AbonementsController : Controller
    {
        private readonly IApiHttpClient _api;

        public AbonementsController(IApiHttpClient api)
        {
            _api = api;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetAbonements();

            var model = await JsonHelper.DeserializeContentAsync<List<AbonementModel>>(response);

            for (int i = 0; i < model.Count; i++)
            {
                model[i].Cost = await JsonHelper.DeserializeContentAsync<decimal>(await _api.GetAbonementsPrice(model[i].Id));
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("{id}/fitnessEvents")]
        public async Task<IActionResult> Index([FromRoute] int id)
        {
            var response = await _api.GetAbonementFitnessEventsByAbonement(id);

            var model = await JsonHelper.DeserializeContentAsync<List<AbonementFitnessEventModel>>(response);

            return new JsonResult(model.Select(m => m.FitnessEventId).ToArray());
        }

        [AllowAnonymous]
        [HttpGet("{id}/price")]
        public async Task<IActionResult> GetAbonementsPrice([FromRoute] int id)
        {
            var response = await JsonHelper.DeserializeContentAsync<decimal>(await _api.GetAbonementsPrice(id));

            return new JsonResult(response);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Info([FromRoute] int id)
        {
            var response = await _api.GetAbonements(id);

            var abonementModel = await JsonHelper.DeserializeContentAsync<AbonementModel>(response);

            var abonementFitnessEvents = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<AbonementFitnessEventModel>>(await _api.GetAbonementFitnessEventsByAbonement(abonementModel.Id));
            var events = (await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<FitnessEventModel>>(await _api.GetFitnessEvents()))
                .Where(f => abonementFitnessEvents.Any(item => item.FitnessEventId == f.Id)).ToList();

            var fitnessEvents = new List<(int, FitnessEventModel)>();

            foreach (var one in events)
            {
                var venueResponse = await _api.GetVenues(one.VenueId);
                var venue = await JsonHelper.DeserializeContentAsync<VenueModel>(venueResponse);
                var serviceResponse = await _api.GetServices(one.ServiceId);
                var service = await JsonHelper.DeserializeContentAsync<ServiceModel>(serviceResponse);

                fitnessEvents.Add((abonementFitnessEvents.First(e => e.FitnessEventId == one.Id).Id, new FitnessEventModel
                {
                    Id = one.Id,
                    Minutes = one.Minutes,
                    VenueId = one.VenueId,
                    ServiceId = one.ServiceId,
                    VenueInfo = $"{venue.Name} ({venue.Location})",
                    ServiceInfo = $"{service.Name}",
                }));
            }

            var model = new AbonementOwnFitnessEventsModel()
            {
                Id = abonementModel.Id,
                Attendances = abonementModel.Attendances,
                Coefficient = abonementModel.Coefficient,
                ImageName = abonementModel.ImageName,
                Name = abonementModel.Name,
                Status = abonementModel.Status,
                FitnessEvents = fitnessEvents,
            };

            model.Cost = await JsonHelper.DeserializeContentAsync<decimal>(await _api.GetAbonementsPrice(model.Id));

            return View(model);
        }


        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("manage")]
        public async Task<IActionResult> Manage()
        {
            var response = await _api.GetAbonements();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<AbonementModel>>(response);

            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("create")]
        public IActionResult Create()
        {

            return View();
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AbonementModel model)
        {
            var response = await _api.PostAbonements(JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("manage");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var response = await _api.GetAbonements(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<AbonementModel>(response);
                return View(model);
            }
            return RedirectToAction("manage");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("edit")]
        public async Task<IActionResult> Update(AbonementModel model)
        {
            var response = await _api.PutAbonements(model.Id, JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetAbonements(id);
            var model = await JsonHelper.DeserializeContentAsync<AbonementModel>(response);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteAbonements(id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var responseContent = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ViewBag.ErrorMessage = responseContent.ErrorMessage;
            return RedirectToAction("Remove", new { id });
        }

        [AllowAnonymous]
        [HttpGet("{id}/image")]
        public async Task<IActionResult> Image([FromRoute] int id)
        {
            var response = await _api.GetAbonementsImage(id);

            if (response.IsSuccessStatusCode)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                return File(content, "image/png");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("{id}/image/change")]
        public IActionResult ChangeImage([FromRoute] int id)
        {
            var model = new ImageUploadModel { Id = id };
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("{id}/image/change")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeImage([FromRoute] int id, ImageUploadModel model)
        {
            if (ModelState.IsValid && model.File != null && model.File.Length > 0)
            {
                HttpResponseMessage response;

                using (var form = new MultipartFormDataContent())
                using (var streamContent = new StreamContent(model.File.OpenReadStream()))
                using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                {
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                    form.Add(fileContent, "file", Path.GetFileName(model.File.FileName));
                    response = await _api.PutAbonementsImage(model.Id, form);
                }

                // "File has not supported extension. Check, if it's an image and try again!"
                return View(new ImageUploadModel { Id = model.Id });
            }

            return RedirectToAction("Index", "Abonements");
        }


        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("availability/{id}")]
        public async Task<IActionResult> ChangeAvailability([FromRoute] int id)
        {
            var response = await _api.GetAbonements(id);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await JsonHelper.DeserializeContentAsync<AbonementModel>(response);
                switch (responseContent.Status)
                {
                    case AbonementStatus.Disabled:
                        {
                            responseContent.Status = AbonementStatus.Enabled;
                            break;
                        }
                    default:
                        {
                            responseContent.Status = AbonementStatus.Disabled;
                            break;
                        }
                }
                await _api.PutAbonements(responseContent.Id, JsonHelper.ObjectToStringContent(responseContent));
            }
            return RedirectToAction("manage");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("{id}/createEvent")]
        public IActionResult CreateEvent([FromRoute] int id)
        {
            var model = new AbonementFitnessEventModel { AbonementId = id };

            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("{id}/createEvent")]
        public async Task<IActionResult> Create([FromRoute] int id, AbonementFitnessEventModel model)
        {
            var response = await _api.PostAbonementFitnessEvents(JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Info", new { id = model.AbonementId });
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("{id}/removeEvent/{eventId}")]
        public async Task<IActionResult> RemoveEvent([FromRoute] int id, [FromRoute] int eventId)
        {
            var abonementsResponse = await _api.GetAbonements(id);
            var abonement = await JsonHelper.DeserializeContentAsync<AbonementOwnFitnessEventsModel>(abonementsResponse);
            var abonementsFitnessEventsResponse = await _api.GetAbonementFitnessEvents(eventId);
            var abonementEvent = await JsonHelper.DeserializeContentAsync<AbonementFitnessEventModel>(abonementsFitnessEventsResponse);

            var fitnessEventResponse = await _api.GetFitnessEvents(abonementEvent.FitnessEventId);
            var fitnessEvent = await JsonHelper.DeserializeContentAsync<FitnessEventModel>(fitnessEventResponse);
            var service = await JsonHelper.DeserializeContentAsync<ServiceModel>(await _api.GetServices(fitnessEvent.ServiceId));
            fitnessEvent.ServiceInfo = $"{service.Name} ({SharedStringRes.CurrencySymbol}{service.Price})";
            var venue = (await JsonHelper.DeserializeContentAsync<VenueModel>(await _api.GetVenues(fitnessEvent.VenueId)));
            fitnessEvent.VenueInfo = $"{venue.Name} ({venue.Location})";

            abonement.FitnessEvents = new List<(int, FitnessEventModel)> { (eventId, fitnessEvent) };
            return View(abonement);
        }

        [HttpGet("confirmRemoveEvent/{id}")]
        public async Task<IActionResult> ConfirmedRemoveEvent([FromRoute] int id)
        {
            var fitevent = await JsonHelper.DeserializeContentAsync<AbonementFitnessEventModel>(await _api.GetAbonementFitnessEvents(id));
            var response = await _api.DeleteAbonementFitnessEvents(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Info", new { id = fitevent.AbonementId });
            }

            var responseContent = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ViewBag.ErrorMessage = responseContent.ErrorMessage;
            return RedirectToAction("RemoveEvent", new { id });
        }
    }
}
