using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FitnessCenterManagement.WebApp.Helpers;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FitnessCenterManagement.WebApp.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly string[] _availableExtensions = { ".png", ".jpg", ".jpeg", ".bmp" };
        private const decimal MinBalance = 0.0m;

        private readonly IApiHttpClient _api;

        public ProfileController(IApiHttpClient api)
        {
            _api = api;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _api.GetProfile();

            if (response.IsSuccessStatusCode)
            {
                var userData = await JsonHelper.DeserializeContentAsync<ProfileModel>(response);

                return View(userData);
            }

            return RedirectToAction("login", "Account");
        }

        [AllowAnonymous]
        [HttpGet("{userId}/avatar")]
        public async Task<IActionResult> UserAvatarImage([FromRoute] string userId)
        {
            var response = await _api.GetProfileImage(userId);

            if (response.IsSuccessStatusCode)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                return File(content, "image/png");
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("image")]
        public async Task<IActionResult> AnonymousUserAvatarImage()
        {
            var response = await _api.GetProfileDefaultImage();

            if (response.IsSuccessStatusCode)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                return File(content, "image/png");
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("{userId}/qr")]
        public async Task<IActionResult> UserQrImage([FromRoute] string userId)
        {
            if (User.IsInRole(Constants.TrainerRole)  || User.IsInRole(Constants.ManagerRole) ||
                User.IsInRole(Constants.DirectorRole) || User.FindFirst(Constants.UserIdClaimType).Value == userId)
            {
                var response = await _api.GetProfileQr(userId);

                if (response.IsSuccessStatusCode)
                {
                    byte[] content = await response.Content.ReadAsByteArrayAsync();

                    return File(content, "image/png");
                }
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("avatar/change")]
        public async Task<IActionResult> ChangeAvatar([FromRoute] string userId)
        {
            return View();
        }

        // POST: /Events/UploadImage?eventId=<eventId>
        [Authorize()]
        [HttpPost("avatar/change")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAvatar(ImageUploadModel model)
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
                    response = await _api.PutProfileImage(form);
                }

                // "File has not supported extension. Check, if it's an image and try again!"
                return View();
            }

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        [HttpGet("avatar")]
        public async Task<IActionResult> CurrentUserAvatarImage()
        {
            var currentUserId = HttpContext.User.FindFirst(Constants.UserIdClaimType).Value;
            return (User.Identity.IsAuthenticated) ? await UserAvatarImage(currentUserId) : NotFound();
        }

        [Authorize]
        [HttpGet("username")]
        public string CurrentUserName()
        {
            var name = HttpContext.User.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
            return name == "" ? Resources.StringResources.UserLabel : name;
        }

        [Authorize]
        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            var response = await _api.GetProfile();

            if (response.IsSuccessStatusCode)
            {
                var userData = await JsonHelper.DeserializeContentAsync<ProfileModel>(response);

                var model = new ProfileEditModel
                {
                    Surname = userData.Surname,
                    FirstName = userData.FirstName,
                    Address = userData.Address,
                    LastName = userData.LastName,
                    Email = userData.Email,
                };
                return View(model);
            }

            return RedirectToAction("login", "Account");
        }

        [Authorize]
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return View(model);
            }

            var profileResponse = await _api.GetProfile();
            var userData = await JsonHelper.DeserializeContentAsync<ProfileModel>(profileResponse);

            if (userData.Email != model.Email)
            {
                var emailIsFree = await _api.GetIsEmailFree(model.Email);
                if (!emailIsFree.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("Email", Resources.StringResources.EmaiIsAlreadyTakenMsg);
                    return View(model);
                }
            }

            using var content = JsonHelper.ObjectToStringContent(model);
            var response = await _api.PutProfile(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var responseContent = await JsonHelper.DeserializeContentAsync<ErrorViewModel>(response);
            ModelState.AddModelError(responseContent.ErrorAttribute, responseContent.ErrorMessage);
            return View(model);
        }

        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> ChangeBalance()
        {
            var response = await _api.GetProfile();

            if (response.IsSuccessStatusCode)
            {
                var userData = await JsonHelper.DeserializeContentAsync<ProfileModel>(response);

                var model = new ProfileChangeBalanceModel { Balance = userData.Balance };

                return View(model);
            }

            return RedirectToAction("Index", "Profile");
        }

        [Authorize]
        [HttpPost("balance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeBalance(ProfileChangeBalanceModel model)
        {
            if (model.Balance < Constants.MinBalance)
            {
                ModelState.AddModelError("Balance", WebApp.Resources.StringResources.NegativeBalance);
                return View(model);
            }

            var response = await _api.PutProfileBalance(JsonHelper.ObjectToStringContent(model));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Profile");
            }
            
            return View(model);
        }
    }
}