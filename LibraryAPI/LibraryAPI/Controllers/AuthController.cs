using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;
using LibraryAPI.Enums;
using LibraryAPI.Services;
using System.Net;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/auth/")]
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

        [HttpHead("session")]
        public async Task<ActionResult> Session()
        {
            try
            {
                if (HttpContext!.User!.Identity!.IsAuthenticated)
                    return Ok();

                return NoContent();
            } catch (Exception ex)
            {
                return NoContent();
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            object code = await _service.LoginAsync(dto);
            return Ok(code);
        }

        [HttpDelete("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _service.Logout();
            return NoContent();
        }
    }
}
