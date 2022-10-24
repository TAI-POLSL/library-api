using LibraryAPI.Enums;
using LibraryAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace LibraryAPI.Services
{
    public class HeaderContextService : IHeaderContextService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpContext GetHttpContext() 
        {
            return _httpContextAccessor.HttpContext;
        }

        public IHeaderDictionary GetRequestHeaders() 
        {
            return GetHttpContext().Request.Headers;
        }

        public ClaimsPrincipal GetUser()
        {
            return GetHttpContext().User;
        }

        public bool IsAuthenticated()
        {
            var ss = GetUser().Identity;

            if (ss != null) 
            {
                return ss.IsAuthenticated;
            }

            return false;
        }

        public Guid GetUserSessionId()
        {
            var claim = GetUser().FindFirstValue("SessionID");
            return new Guid(claim);
        }

        public Guid GetUserId()
        {
            var claim = GetUser().FindFirstValue(ClaimTypes.NameIdentifier);
            return new Guid(claim);
        }

        public string GetUserUsername()
        {
            var claim = GetUser().FindFirstValue(ClaimTypes.Name);
            return claim;
        }

        public UserRoles GetUserRole()
        {
            var claim = GetUser().FindFirstValue(ClaimTypes.Role);
            return (UserRoles)Enum.Parse(typeof(UserRoles), claim, true);
        }

        public string GetUserRemoteIpAddress() 
        { 
            return GetHttpContext().Connection.RemoteIpAddress.ToString();
        } 

    }
}
