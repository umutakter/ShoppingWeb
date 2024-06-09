using CoreLibrary.Attributes.Interfaces;
using CoreLibrary.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoppingML.Attributes
{
    public class CoreAuthorizationAttribute : Attribute
    {
        public CoreAuthorizationAttribute()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var actionName = context.ActionDescriptor.RouteValues["action"];

            if (TokenValidationService.ValidateToken(token, out var licenseKey, out var permissions))
            {
                if (permissions.Contains(actionName))
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }
}