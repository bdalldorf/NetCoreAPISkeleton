using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IConfiguration>(Configuration);

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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiConstants.ApiVersion}/swagger.json", $"{ApiConstants.Title} {ApiConstants.ApiVersion}");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
