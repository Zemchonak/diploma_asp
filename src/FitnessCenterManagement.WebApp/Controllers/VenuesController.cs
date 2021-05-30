using FitnessCenterManagement.WebApp.Helpers;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
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
    public class VenuesController : Controller
    {
        private readonly IApiHttpClient _api;

        public VenuesController(IApiHttpClient api)
        {
            _api = api;
        }

        [HttpGet("find")]
        [AllowAnonymous]
        public async Task<IActionResult> Find(string part)
        {
            var response = await _api.GetVenues(part);
            var content = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<VenueModel>>(response);
            return new JsonResult(content.ToList());
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetVenues();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<VenueModel>>(response);

            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(VenueModel model)
        {
            var response = await _api.PostVenues(JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetVenues(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<VenueModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("edit")]
        public async Task<IActionResult> Update(VenueModel model)
        {
            var response = await _api.PutVenues(model.Id, JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetVenues(id);
            var model = await JsonHelper.DeserializeContentAsync<VenueModel>(response);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteVenues(id);

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
            var response = await _api.GetVenuesImage(id);

            if (response.IsSuccessStatusCode)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                return File(content, "image/png");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("{id}/image/change")]
        public async Task<IActionResult> ChangeImage([FromRoute] int id)
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
                    response = await _api.PutVenuesImage(model.Id, form);
                }

                // "File has not supported extension. Check, if it's an image and try again!"
                return View(new ImageUploadModel { Id = model.Id });
            }

            return RedirectToAction("Index", "Venues");
        }

        [Authorize(Roles = Constants.TrainerRole + "," + Constants.ManagerRole + "," + Constants.DirectorRole)]
        [HttpGet("{venueId}/qr")]
        public async Task<IActionResult> VenueQrImage([FromRoute] int venueId)
        {
            var response = await _api.GetVenuesQr(venueId);

            if (response.IsSuccessStatusCode)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                return File(content, "image/png");
            }

            return NotFound();
        }
    }
}
