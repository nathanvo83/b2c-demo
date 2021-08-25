using common.Customise;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    public static class Extensions
    {
        public static void AddApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var role in PolicyConstants.Policies)
                {
                    options.AddPolicy(role, policy =>
                    {
                        policy.RequireClaim("extension_Role", role);
                    });
                }

                //options.AddPolicy(PolicyConstants.Admin, policy =>
                //{
                //    policy.RequireClaim("extension_Role", PolicyConstants.Admin);
                //});

                //options.AddPolicy(PolicyConstants.User, policy =>
                //{
                //    policy.RequireClaim("extension_Role", PolicyConstants.User);
                //});

                //options.AddPolicy(PolicyConstants.Manager, policy =>
                //{
                //    policy.RequireClaim("extension_Role", PolicyConstants.Manager);
                //});
            });

            services.AddSingleton<IAuthorizationPolicyProvider, PoliciesProvider>();
            services.AddSingleton<IAuthorizationHandler, PoliciesAuthorizationHandler>();
        }

        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => configuration.Bind("AzureAdB2C", options));
        }
    }
}
