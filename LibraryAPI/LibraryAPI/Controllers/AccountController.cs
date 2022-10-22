using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/account/")]
    public class AccountController : ControllerBase
    {

        public AccountController()
        {
        }


        [HttpPost]
        public ActionResult Register()
        {
            return Ok();
        }

        [HttpPatch]
        public ActionResult Lock()
        {
            return Ok();
        }

        [HttpPatch("password")]
        public ActionResult ChangePassword()
        {
            return Ok();
        }

        [HttpDelete]
        public ActionResult Close()
        {
            return Ok();
        }
    }
}
