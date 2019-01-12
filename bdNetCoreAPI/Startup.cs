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
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Events.OnRedirectToLogin = redirectContext =>
                    {
                        //if (!(IsWebRequest(redirectContext.Request) || IsApiRequest(redirectContext.Request)))
                        //{
                        //    redirectContext.Response.Redirect(redirectContext.RedirectUri);
                        //    return Task.CompletedTask;
                        //}
                        redirectContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
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
                //options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;

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
