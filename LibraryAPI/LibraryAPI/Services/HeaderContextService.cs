using LibraryAPI.Interfaces;
using Microsoft.Extensions.Primitives;

namespace LibraryAPI.Services
{
    public class HeaderContextService : IHeaderContextService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHeaderDictionary Header => _httpContextAccessor.HttpContext?.Request.Headers;
        public string RemoteIpAddress() { 
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
        } 

    }
}
