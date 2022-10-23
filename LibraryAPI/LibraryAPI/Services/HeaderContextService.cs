using LibraryAPI.Interfaces;
using Microsoft.AspNetCore.Http;
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

        public HttpContext HttpContext => _httpContextAccessor.HttpContext;

        public IHeaderDictionary Header => _httpContextAccessor.HttpContext?.Request.Headers;

        public string RemoteIpAddress() { 
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
        } 

    }
}
