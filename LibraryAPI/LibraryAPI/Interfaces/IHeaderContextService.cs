using LibraryAPI.Enums;
using System.Security.Claims;

namespace LibraryAPI.Interfaces
{
    public interface IHeaderContextService
    {
        public HttpContext GetHttpContext();
        public IHeaderDictionary GetRequestHeaders();
        public ClaimsPrincipal GetUser();
        public bool IsAuthenticated();
        public Guid GetUserSessionId();
        public Guid GetUserId();
        public string GetUserUsername();
        public UserRoles GetUserRole();
        public string GetUserRemoteIpAddress();

    }
}
