using System.Security.Claims;
using RocketLunch.domain.models;

namespace RocketLunch.domain.contracts
{
    public interface IManageClaims
    {
        Identity GetIdentityFromClaims(ClaimsPrincipal user);
    }
}