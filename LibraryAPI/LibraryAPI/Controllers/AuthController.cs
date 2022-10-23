using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;

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
        public ActionResult Login([FromBody] LoginDto dto)
        {
            object code = _service.Login(dto);
            return Ok(code);
        }

        [HttpDelete("logout")]
        public async Task<ActionResult> Logout()
        {
            object code = await _service.Logout();
            return Ok(code);
        }
    }
}
