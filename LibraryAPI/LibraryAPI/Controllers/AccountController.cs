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
        public object GetById([FromRoute] Guid userId)
        {
            var obj = _service.Get(userId);
            return Ok(obj);
        }

        [HttpGet("account/{userId}/audit")]
        public object GetAuditByUserId([FromRoute] Guid userId)
        {
            var obj = _service.GetAuditByUserId(userId);
            return Ok(obj);
        }

        [HttpPost("account")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            
            var obj = await _service.Register(dto);
            return Ok(obj);
        }

        [HttpPatch("account/{userId}/lock")]
        public ActionResult Lock(Guid userId)
        {
            var obj = _service.Lock(userId);
            return Ok(obj);
        }

        [HttpPatch("account/{userId}/password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto, [FromRoute] Guid userId)
        {
            var obj = await _service.ChangePassword(dto, userId);
            return Ok(obj);
        }

        [HttpDelete("account/{userId}/close")]
        public ActionResult Close(Guid userId)
        {
            var obj = _service.Close(userId);
            return Ok(obj);
        }
    }
}
