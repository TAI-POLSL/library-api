using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryAPI
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public CustomCookieAuthenticationEvents(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            // Look for the SessionId claim.
            var sessionId = (from c in userPrincipal.Claims
                               where c.Type == "SessionID"
                               select c.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(sessionId))
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            } else
            {
                // Check wether SESSION is valid

                var guidSessionId = new Guid(sessionId);
                var ex = _context.Sessions.AsNoTracking().Where(x => x.Id == guidSessionId).FirstOrDefault();

                if(ex == null)
                {
                    await _authService.Logout();
                }

            }
        }
    }
}
