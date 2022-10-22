using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/auth/")]
    public class AuthController : ControllerBase
    {

        private readonly string _appBaseUrl;

        public AuthController(
            IConfiguration configuration
        ) {
            _appBaseUrl = configuration.GetValue<string>("AppUrl");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login()
        {
            return Ok();
        }

        [HttpDelete("logout")]
        public ActionResult Logout()
        {
            return Ok();
        }
    }
}
