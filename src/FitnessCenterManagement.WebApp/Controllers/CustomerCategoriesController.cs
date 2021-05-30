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
    [Authorize(Roles = Constants.MarketerRole)]
    [Route("[controller]")]
    public class CustomerCategoriesController : Controller
    {
        private readonly IApiHttpClient _api;

        public CustomerCategoriesController(IApiHttpClient api)
        {
            _api = api;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetCustomerCategories();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<CustomerCategoryModel>>(response);

            return View(model);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var model = new CustomerCategoriesEditModel();
            return View(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CustomerCategoriesEditModel model)
        {
            var response = await _api.PostCustomerCategories(JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetCustomerCategories(id);
            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<CustomerCategoriesEditModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Update(CustomerCategoriesEditModel model)
        {
            var response = await _api.PutCustomerCategories(model.Id, JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetCustomerCategories(id);
            var model = await JsonHelper.DeserializeContentAsync<CustomerCategoryModel>(response);
            return View(model);
        }

        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteCustomerCategories(id);
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
