﻿using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace FitnessCenterManagement.WebApp.Helpers
{
    public static class RequestHelper
    {
        internal const string JwtCookiesKey = "secret_jwt_key";

        public static void SetRequestToken(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            if (client is null)
            {
                throw new System.ArgumentNullException(nameof(client));
            }

            var token = httpContextAccessor?.HttpContext?.Request.Cookies[JwtCookiesKey];

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
