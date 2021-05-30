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

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<AbonementModel>>(response);

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
        public async Task<IActionResult> Create()
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
    }
}
