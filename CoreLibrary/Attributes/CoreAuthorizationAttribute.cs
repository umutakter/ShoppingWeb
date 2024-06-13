using CoreLibrary.Attributes.Interfaces;
using CoreLibrary.Authorization;
using CoreLibrary.Authorization.Interfaces;
using CoreLibrary.Models;
using CoreLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace ShoppingML.Attributes
{
    public class CoreAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authorizationHeader.Replace("Bearer ", "");
            var tokenValidationService = new TokenValidationService();
            if (string.IsNullOrEmpty(token) || !tokenValidationService.ValidateToken(token, out string licenseKey, out string[] permissions, out string expireDate))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if(licenseKey == "" ||permissions.Length == 0|| DateTime.Parse(expireDate) < DateTime.Now)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var routeData = context.ActionDescriptor.RouteValues;
            var controllerName = routeData["controller"];
            var actionName = routeData["action"];

            // Assembly'den proje adını al
            string assemblyName = Assembly.GetEntryAssembly()!.GetName().Name!;

            var apiFullName = $"{assemblyName}.{controllerName}Controller.{actionName}";

            if (!permissions.Contains(apiFullName))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}