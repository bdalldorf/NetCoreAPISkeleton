using bdNetCoreAPIDataTransfer;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using System.Threading.Tasks;

namespace bdNetCoreAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {
                //options.Cookie.Name = "auth_cookie";
                //options.Cookie.SameSite = SameSiteMode.None;
                options.Events.OnRedirectToLogin = redirectContext =>
                    {
                        //if (!(IsWebRequest(redirectContext.Request) || IsApiRequest(redirectContext.Request)))
                        //{
                        //    redirectContext.Response.Redirect(redirectContext.RedirectUri);
                        //    return Task.CompletedTask;
                        //}
                        redirectContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return redirectContext.Response.WriteAsync("Unauthorized");
                    };
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = ApiConstants.ValidateIssuer,
                        ValidateAudience = ApiConstants.ValidateAudience,
                        ValidateLifetime = ApiConstants.ValidateLifetime,
                        ValidateIssuerSigningKey = ApiConstants.ValidateIssuerSigningKey,
                        ValidIssuer = ApiConstants.ValidIssuer,
                        ValidAudience = ApiConstants.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                    };
                });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiConstants.ApiVersion, new Info
                {
                    Title = ApiConstants.Title,
                    Version = ApiConstants.ApiVersion,
                    Description = ApiConstants.ApiDescription,
                    TermsOfService = ApiConstants.ApiTermsOfService,
                    Contact = new Contact
                    {
                        Name = "Bret Dalldorf",
                        Email = string.Empty,
                        Url = "https://github.com/bdalldorf/NetCoreAPISkeleton"
                    },
                    License = new License
                    {
                        Name = "None",
                        Url = ""
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IAntiforgery antiforgery, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Shows UseCors with named policy.
            app.UseCors("CorsPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiConstants.ApiVersion}/swagger.json", $"{ApiConstants.Title} {ApiConstants.ApiVersion}");
                c.RoutePrefix = string.Empty;
            });

            app.Use(next => context =>
            {
                if (
                    string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
                {
                    // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions() { HttpOnly = false });
                }

                return next(context);
            });


            app.UseMvc();
            app.UseStaticFiles();
        }

        private static bool IsWebRequest(HttpRequest request)
        {
            var query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        private static bool IsApiRequest(HttpRequest request)
        {
            return request.Path.StartsWithSegments(new PathString("/api"));
        }
    }
}
