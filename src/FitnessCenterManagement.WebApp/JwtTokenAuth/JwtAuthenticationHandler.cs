using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using FitnessCenterManagement.WebApp.Helpers;

namespace FitnessCenterManagement.WebApp
{
    internal class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IApiHttpClient _apiHttpClient;

        public JwtAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            ISystemClock clock,
            IApiHttpClient apiHttpClient,
            UrlEncoder encoder)
            : base(options, logger, encoder, clock)
        {
            _apiHttpClient = apiHttpClient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey(RequestHelper.JwtCookiesKey))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var token = Request.Cookies[RequestHelper.JwtCookiesKey].ToString(null);

            var response = await _apiHttpClient.GetUsersValidateToken(token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}
