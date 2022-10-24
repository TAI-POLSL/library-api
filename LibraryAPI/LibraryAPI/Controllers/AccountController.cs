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
        [Authorize(Roles = "ADMIN")]
        public object Get() 
        { 
            var obj = _service.Get();
            return Ok(obj);
        }

        [HttpGet("account/{userId}")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public object GetById([FromRoute] Guid userId)
        {
            // TODO EMPLOYEE can get only CLIENTS accounts
            var obj = _service.Get(userId);
            return Ok(obj);
        }

        [HttpGet("account/{userId}/audit")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public object GetAuditByUserId([FromRoute] Guid userId)
        {
            // TODO EMPLOYEE and CLIENTS can get only own audits
            var obj = _service.GetAuditByUserId(userId);
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
            // TODO EMPLOYEE can only lock CLIENTS accounts
            var obj = _service.Lock(userId);
            return Ok(obj);
        }

        [HttpPatch("account/{userId}/password")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto, [FromRoute] Guid userId)
        {
            // TODO EMPLOYEE can only change CLIENTS accounts
            var obj = await _service.ChangePassword(dto, userId);
            return Ok(obj);
        }

        [HttpDelete("account/{userId}/close")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult Close(Guid userId)
        {
            // TODO EMPLOYEE can only close CLIENTS accounts
            // TODO One ADMIN account is nessesery
            var obj = _service.Close(userId);
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
