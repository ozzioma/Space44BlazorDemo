using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorServerApp.Areas.Identity;
using BlazorServerApp.Config;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Persistence;
using Refit;
using Syncfusion.Blazor;

namespace BlazorServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

           

            int sessionTimeOut = int.Parse(Configuration["SESSION_TIMEOUT_MINS"]);

            services.AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme, config =>
                    {
                        config.Cookie.Name = ConfigKeys.WEB_COOKIE_NAME;
                        config.Cookie.IsEssential = true;
                        //config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        config.Cookie.SameSite = SameSiteMode.Lax;
                        config.LoginPath = "/Identity/Account/Login2";
                        config.LogoutPath = new PathString("/Identity/Account/LogOut2");

                        config.SlidingExpiration = true;
                        config.Cookie.HttpOnly = true;
                        config.ExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeOut);
                        config.ReturnUrlParameter = "/"; // CookieAuthenticationDefaults.ReturnUrlParameter;

                    });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.ConsentCookie.IsEssential = true;
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });


            int maxRequestLimit = int.Parse(Configuration["MaxRequestBodySize"]);
            // If using IIS
            services.Configure<IISServerOptions>(options => { options.MaxRequestBodySize = maxRequestLimit; });
            // If using Kestrel
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = maxRequestLimit;
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = maxRequestLimit;
                x.MultipartBodyLengthLimit = maxRequestLimit;
                x.MultipartHeadersLengthLimit = maxRequestLimit;
            });


            services.AddAuthorization();
            // services.AddAuthentication();

            services.AddSession();

            services.AddMvc(options =>
                {
                    //options.Filters.Add(new AuthorizeFilter());
                    // var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    // options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    //options.SerializerSettings.MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling =
                        Newtonsoft.Json.PreserveReferencesHandling.None;
                    // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddFluentValidation(options =>
                {
                    //options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    options.DisableDataAnnotationsValidation = true;
                });

            services.AddRazorPages(options => { }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            });

            services.AddServerSideBlazor().AddCircuitOptions(options =>
            {
                //options.DetailedErrors = true;
            });
            services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });

            services
                .AddScoped<AuthenticationStateProvider,
                    RevalidatingIdentityAuthenticationStateProvider<AppUser>>();

            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAntDesign();
            services.AddSyncfusionBlazor();

            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();

            services.AddScoped<WebConfigHelper>();
            services.AddScoped<BrowserService>();
            services.AddScoped<RefitDelegatingHandler>();
            
            var settings = new RefitSettings();
            services.AddRefitClient<EntityDataService>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration[ConfigKeys.API_HOST]))
                .AddHttpMessageHandler<RefitDelegatingHandler>();

            services.AddRefitClient<AuthenticationService>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration[ConfigKeys.API_HOST]));

            services.AddScoped<HttpClient>(s =>
            {
                return new HttpClient {BaseAddress = new Uri(Configuration[ConfigKeys.API_HOST])};
            });
            

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            //19.3
            //
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTE2ODk4QDMxMzkyZTMzMmUzMG5nUkdNdUY4MURIamRwUUhaTUg4N1RCVDhaaWc0UVA5SjVJZi9XeXRtMGM9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}