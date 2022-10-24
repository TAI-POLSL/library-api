using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet("accounts")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public object Get() 
        {
            // EMPLOYEE can get only CLIENTS accounts
            var obj = _service.GetStrategy();
            return Ok(obj);
        }

        [HttpGet("account/{userId}")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public object GetById([FromRoute] Guid userId)
        {
            // EMPLOYEE can get only CLIENTS accounts
            var obj = _service.GetStrategy(userId);
            return Ok(obj);
        }

        [HttpGet("account/own")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public object GetOwn()
        {
            var obj = _service.GetOwn();
            return Ok(obj);
        }

        [HttpPost("account")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            // EMPLOYEE can only register CLIENTS accounts
            var obj = await _service.Register(dto);
            return Ok(obj);
        }

        [HttpPatch("account/{userId}/lock")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult Lock(Guid userId)
        {
            // EMPLOYEE can only lock CLIENTS accounts
            var obj = _service.ChangeAccountLockStatus(userId, true);
            return Ok(obj);
        }

        [HttpPatch("account/{userId}/unlock")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult Unlock(Guid userId)
        {
            // EMPLOYEE can unlock lock CLIENTS accounts
            var obj = _service.ChangeAccountLockStatus(userId, false);
            return Ok(obj);
        }

        [HttpPatch("account/passwd")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var obj = await _service.ChangePassword(dto);
            return Ok(obj);
        }

        [HttpDelete("account/{userId}/close")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult Close(Guid userId)
        {
            // EMPLOYEE can only close CLIENTS accounts
            // One ADMIN account is nessesery
            var obj = _service.CloseStrategy(userId);
            return Ok(obj);
        }

        [HttpDelete("account/close")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENTS")]
        public ActionResult CloseContextAccount()
        {
            var obj = _service.CloseStrategy(null);
            return Ok(obj);
        }

        // TODO Seed account
        [AllowAnonymous]
        [HttpPost("account/generate/admin")]
        public async Task<ActionResult> GenerateAdmin()
        {
            var obj = await _service.GenerateAdmin();
            return Ok(obj);
        }
    }
}
