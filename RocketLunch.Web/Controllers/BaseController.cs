using System;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.contracts;
using RocketLunch.domain.models;

namespace RocketLunch.web.controllers
{
    public class BaseController: Controller
    {
        private IManageClaims _claims;

		public BaseController(IManageClaims claims)
		{
			_claims = claims ?? throw new ArgumentNullException("IManageClaims");
		}

        public Identity GetIdentityFromClaims()
		{
			return _claims.GetIdentityFromClaims(Request.HttpContext.User);
		}
    }
}