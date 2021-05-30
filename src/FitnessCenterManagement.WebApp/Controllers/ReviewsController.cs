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
    public class ReviewsController : Controller
    {
        private readonly IApiHttpClient _api;

        public ReviewsController(IApiHttpClient api)
        {
            _api = api;
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string part)
        {
            var response = await _api.GetReviews(part);
            var content = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<ReviewModel>>(response);
            return new JsonResult(content.ToList());
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetReviews();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<ReviewModel>>(response);

            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("manage")]
        public async Task<IActionResult> Manage()
        {
            var response = await _api.GetReviews();

            var model = await JsonHelper.DeserializeContentAsync<IReadOnlyCollection<ReviewModel>>(response);

            return View(model);
        }

        [Authorize]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(ReviewEditModel model)
        {
            if(ModelState.IsValid)
            {
                model.IsHidden = true;
                model.UserId = User.FindFirst(Constants.UserIdClaimType).Value;
                var response = await _api.PostReviews(JsonHelper.ObjectToStringContent(model));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                var content = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
                ModelState.AddModelError(content.ErrorAttribute, content.ErrorMessage);
                return View(model);
            }
            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("edit")]
        public async Task<IActionResult> Update()
        {
            var userId = User.FindFirst(Constants.UserIdClaimType).Value;
            var response = await _api.GetReviewByAuthorId(userId);

            if (response.IsSuccessStatusCode)
            {
                var model = await JsonHelper.DeserializeContentAsync<ReviewEditModel>(response);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost("edit")]
        public async Task<IActionResult> Update(ReviewEditModel model)
        {
            model.IsHidden = true;
            model.UserId = User.FindFirst(Constants.UserIdClaimType).Value;

            var response = await _api.PutReviews(model.Id, JsonHelper.ObjectToStringContent(model));
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
            var response = await _api.GetReviews(id);
            var model = await JsonHelper.DeserializeContentAsync<ReviewModel>(response);
            return View(model);
        }

        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("confirmRemove/{id}")]
        public async Task<IActionResult> ConfirmedRemove([FromRoute] int id)
        {
            var response = await _api.DeleteReviews(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("manage");
            }

            var responseContent = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ViewBag.ErrorMessage = responseContent.ErrorMessage;
            return RedirectToAction("Remove", new { id });
        }
        
        [Authorize(Roles = Constants.MarketerRole)]
        [HttpGet("visibility/{id}")]
        public async Task<IActionResult> ChangeVisibility([FromRoute] int id)
        {
            var response = await _api.GetReviews(id);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await JsonHelper.DeserializeContentAsync<ReviewModel>(response);
                responseContent.IsHidden = !responseContent.IsHidden;
                await _api.PutReviews(responseContent.Id, JsonHelper.ObjectToStringContent(responseContent));
            }
            return RedirectToAction("manage");
        }
    }
}