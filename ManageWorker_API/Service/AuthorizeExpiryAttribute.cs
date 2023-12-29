using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ManageWorker_API.Service
{
    public class AuthorizeExpiryAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var tokenString = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(tokenString))
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(tokenString.Replace("Bearer ", "")) as JwtSecurityToken;

            if (tokenS is not null && tokenS.ValidTo < DateTime.UtcNow)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
}