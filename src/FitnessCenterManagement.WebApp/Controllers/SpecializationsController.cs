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
    public class SpecializationsController : Controller
    {
        private readonly IApiHttpClient _api;

        public SpecializationsController(IApiHttpClient api)
        {
            _api = api;
        }

        [HttpGet("find")]
        [Authorize(Roles = Constants.ManagerRole
            + "," + Constants.MarketerRole
            + "," + Constants.DirectorRole)]
        public async Task<IActionResult> Find(string part)
        {
            var response = await _api.GetSpecializations(part);
            var content = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<SpecializationModel>>(response);
            return new JsonResult(content.ToList());
        }

        [Authorize(Roles = Constants.ManagerRole
            + "," + Constants.TrainerRole
            + "," + Constants.MarketerRole
            + "," + Constants.DirectorRole)]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetSpecializations();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<SpecializationModel>>(response);

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
        public async Task<IActionResult> Create(SpecializationModel model)
        {
            var response = await _api.PostSpecializations(JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetSpecializations(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<SpecializationModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpPost("edit")]
        public async Task<IActionResult> Update(SpecializationModel model)
        {
            var response = await _api.PutSpecializations(model.Id, JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetSpecializations(id);
            var model = await JsonHelper.DeserializeContentAsync<SpecializationModel>(response);
            return View(model);
        }

        [Authorize(Roles = Constants.ManagerRole)]
        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteSpecializations(id);
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
