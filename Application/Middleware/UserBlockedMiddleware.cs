using Application.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Middleware
{
    public class UserBlockedMiddleware
    {
        private readonly RequestDelegate _next;

        public UserBlockedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await userManager.FindByIdAsync(id);

                if (user != null && user.IsBlocked)
                {
                    await signInManager.SignOutAsync();
                    httpContext.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
