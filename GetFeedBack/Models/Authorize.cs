using GetFeedBack.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;

namespace GetFeedBack.Models
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params string[] claim) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }

        public class AuthorizeFilter : IAuthorizationFilter
        {
            readonly string[] _claim;

            public AuthorizeFilter(params string[] claim)
            {
                _claim = claim;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
                var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (IsAuthenticated)
                {
                    bool flagClaim = false;
                    foreach (var item in _claim)
                    {
                        if (context.HttpContext.User.HasClaim("Role", item))
                            flagClaim = true;
                    }
                    if (!flagClaim)
                    {
                        context.Result = new RedirectResult("~/Account/Login");
                    }
                }
                else
                {
                    context.Result = new RedirectResult("~/Account/Login");
                }
                return;
            }
        }
    }
}
