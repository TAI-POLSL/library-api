using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/auth/")]
    public class AuthController : ControllerBase
    {

        private readonly string _appBaseUrl;
        private readonly IAuthService _service;

        public AuthController(
            IConfiguration configuration,
            IAuthService service
        ) {
            _appBaseUrl = configuration.GetValue<string>("AppUrl");
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login()
        {
            string code = _service.Login();
            return Ok(code);
        }

        [HttpDelete("logout")]
        public async Task<ActionResult> Logout()
        {
            string code = await _service.Logout();
            return Ok(code);
        }
    }
}
