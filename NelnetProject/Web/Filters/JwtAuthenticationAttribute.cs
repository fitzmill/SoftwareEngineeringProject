using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Web.Filters
{
    /*
     * Mostly borrowed from https://github.com/cuongle/WebApi.Jwt
     */
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public UserType[] Roles { get; set; } = new UserType[] { };
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);

            else
                context.Principal = principal;
        }



        private bool ValidateToken(string token, out IList<Claim> claims)
        {
            claims = null;

            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var roleClaim = identity.FindFirst(ClaimTypes.Role);
            var role = roleClaim?.Value;

            if (role == null || (this.Roles.Length > 0 && !this.Roles.Any(x => x.ToString() == roleClaim?.Value)))
            {
                return false;
            }

            var nameClaim = identity.FindFirst(ClaimTypes.Name);

            var emailClaim = identity.FindFirst(ClaimTypes.Email);

            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var customerIdClaim = identity.FindFirst("CustomerID");

            if (roleClaim == null || nameClaim == null || emailClaim == null || userIdClaim == null || customerIdClaim == null)
            {
                return false;
            }

            claims = new List<Claim>(new Claim[] { userIdClaim, nameClaim, emailClaim, roleClaim, customerIdClaim });

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            if (ValidateToken(token, out IList<Claim> claims))
            {
                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}