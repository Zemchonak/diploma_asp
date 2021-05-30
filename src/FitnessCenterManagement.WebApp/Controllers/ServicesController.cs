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
    public class ServicesController : Controller
    {
        private readonly IApiHttpClient _api;

        public ServicesController(IApiHttpClient api)
        {
            _api = api;
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string part)
        {
            var response = await _api.GetServices(part);
            var content = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<ServiceModel>>(response);
            return new JsonResult(content.ToList());
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetServices();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<ServiceModel>>(response);

            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(ServiceEditingModel model)
        {
            var response = await _api.PostServices(JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var response = await _api.GetServices(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<ServiceEditingModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpPost("edit")]
        public async Task<IActionResult> Update(ServiceEditingModel model)
        {
            var response = await _api.PutServices(model.Id, JsonHelper.ObjectToStringContent(model));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var response = await _api.GetServices(id);
            var model = await JsonHelper.DeserializeContentAsync<ServiceModel>(response);
            var spec = await _api.GetSpecializations(model.SpecializationId);
            var specInfo = (await JsonHelper.DeserializeContentAsync<SpecializationModel>(spec)).Info;
            model.SpecializationInfo = specInfo;
            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteServices(id);
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
