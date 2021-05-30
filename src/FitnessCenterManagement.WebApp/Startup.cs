using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FitnessCenterManagement.WebApp.HttpClients;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Interfaces;
using FitnessCenterManagement.WebApp.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FitnessCenterManagement.WebApp
{
    public class Startup
    {
        private static readonly Lazy<ICultureProvider> LazyCultureProvider = new Lazy<ICultureProvider>(
            () => new CultureProvider(new CultureProviderFactory()));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, null);

            services.AddControllersWithViews();
            services.AddHttpClient<IApiHttpClient, ApiHttpClient>(c =>
            {
                c.BaseAddress = new Uri(Configuration["ApiAddress"]);
            });

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = LazyCultureProvider.Value.Cultures.Select(e => e).ToList();

                options.DefaultRequestCulture = new RequestCulture(culture: "ru-RU", uiCulture: "ru-RU");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                {
                    var cookiesLang = context.Request.Cookies["Language"];

                    var browserLangCode = context.Request.GetTypedHeaders().AcceptLanguage.First().Value.ToString()
                            .Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();

                    var browserLang = supportedCultures.FirstOrDefault(lang =>
                        lang.Name.Contains(browserLangCode, StringComparison.InvariantCultureIgnoreCase)).Name;

                    return await Task.FromResult(new ProviderCultureResult(cookiesLang is null ? browserLang : cookiesLang));
                }));
            });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/Home/Error");

            app.UseRequestLocalization();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
