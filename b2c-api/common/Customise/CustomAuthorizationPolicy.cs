using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Customise
{
    internal class PoliciesValues
    {
        internal static readonly string POLICY_PREFIX = "XYZ";
        internal static readonly string SEPARATOR = "_";
    }

    internal class PoliciesRequirement : IAuthorizationRequirement
    {
        public string[] Policies { get; private set; }

        public PoliciesRequirement(string[] policies)
        {
            Policies = policies;
        }
    }

    internal class PoliciesAuthorizationHandler : AuthorizationHandler<PoliciesRequirement>
    {
        private readonly ILogger<PoliciesAuthorizationHandler> _logger;

        public PoliciesAuthorizationHandler(ILogger<PoliciesAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PoliciesRequirement requirement)
        {
            var policies = requirement.Policies;
            foreach (var policy in policies)
            {
                var x = context.User.Claims.FirstOrDefault(claim => claim.Type == "extension_Role" && claim.Value == policy);

                if (x != null)
                {
                    _logger.LogInformation("claim => found: " + x.Value);
                    context.Succeed(requirement);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }

    public class AuthorizePoliciesAttribute : AuthorizeAttribute
    {
        public AuthorizePoliciesAttribute(params string[] policyList) => PolicyList = policyList;

        public string[] PolicyList
        {
            get
            {
                if (Policy.StartsWith(PoliciesValues.POLICY_PREFIX))
                {
                    return Policy.Substring(PoliciesValues.POLICY_PREFIX.Length).Split(PoliciesValues.SEPARATOR);
                }

                return null;
            }
            set
            {
                var temp = string.Join(PoliciesValues.SEPARATOR, value);
                Policy = $"{PoliciesValues.POLICY_PREFIX}{temp}";
            }
        }
    }

    internal class PoliciesProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PoliciesProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PoliciesValues.POLICY_PREFIX))
            {
                var value = policyName.Substring(PoliciesValues.POLICY_PREFIX.Length).Split(PoliciesValues.SEPARATOR);

                var policyBuilder = new AuthorizationPolicyBuilder();
                policyBuilder.AddRequirements(new PoliciesRequirement(value));
                return Task.FromResult(policyBuilder.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
