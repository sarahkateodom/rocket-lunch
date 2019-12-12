using System;
using System.Linq;
using System.Security.Claims;
using RocketLunch.domain.contracts;
using RocketLunch.domain.exceptions;
using RocketLunch.domain.models;

namespace RocketLunch.domain.services
{
    public class ClaimsService : IManageClaims
    {
        public Identity GetIdentityFromClaims(ClaimsPrincipal user)
        {
            if (user == null || !user.Claims.Any()) throw new NotAuthorizedException("Access denied. User is required.");
            var claims = user.Claims;

            // get Id
            var claimsId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
            Int32.TryParse(claimsId.Value, out int id);

            // get Name
            var claimsName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            return new Identity()
            {
                Id = id,
                Name = claimsName?.Value
            };
        }
    }
}