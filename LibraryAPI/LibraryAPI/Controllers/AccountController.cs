using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/account/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }


        [HttpPost]
        public ActionResult Register()
        {
            var obj = _service.Register();
            return Ok(obj);
        }

        [HttpPatch("{userId}")]
        public ActionResult Lock(Guid userId)
        {
            var obj = _service.Lock(userId);
            return Ok(obj);
        }

        [HttpPatch("password")]
        public ActionResult ChangePassword()
        {
            var obj = _service.ChangePassword();
            return Ok(obj);
        }

        [HttpDelete]
        public ActionResult Close()
        {
            var obj = _service.Close();
            return Ok(obj);
        }
    }
}
