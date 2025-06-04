using MI.Dapper.Data.Constant;
using System;
using System.Linq;
using System.Security.Claims;

namespace PlatformCMS.Extensions
{
    public static class IdentityExtension
    {
        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

            return (claim != null) ? claim.Value : string.Empty;
        }

        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x =>
               x.Type == SystemConstants.UserClaim.Id);
            return Guid.Parse(claim.Value);
        }

        public static string GetRoleName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x =>
               x.Type == SystemConstants.UserClaim.Roles);
            return claim.Value;
        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.SingleOrDefault(x =>
                x.Type == SystemConstants.UserClaim.FullName);

            if (claim == null)
                return null;

            return claim.Value;
        }

    }
}
