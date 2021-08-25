using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using common;

namespace auth_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // All localhost
                options.AddPolicy("localhost", builder =>
                {
                    builder
                        .SetIsOriginAllowed(x => new Uri(x).Host == "localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                options.AddPolicy("custom", builder =>
                {
                    var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>();
                    if (allowedOrigins != null)
                    {
                        builder
                            .WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                });
            });

            services.AddApiAuthentication(Configuration);
            services.AddApiAuthorization();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("localhost");
            app.UseCors("custom");

            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                var x = context.User.Identity.IsAuthenticated;
                var y = context.User.Claims;
                await next();
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
