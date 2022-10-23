using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;

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

        [HttpGet()]
        public object Get() 
        { 
            var obj = _service.Get();
            return Ok(obj);
        }

        [HttpGet("{userId}")]
        public object GetById([FromRoute] Guid userId)
        {
            var obj = _service.Get(userId);
            return Ok(obj);
        }

        [HttpGet("{userId}/audit")]
        public object GetAuditByUserId([FromRoute] Guid userId)
        {
            var obj = _service.GetAuditByUserId(userId);
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            
            var obj = await _service.Register(dto);
            return Ok(obj);
        }

        [HttpPatch("{userId}/lock")]
        public ActionResult Lock(Guid userId)
        {
            var obj = _service.Lock(userId);
            return Ok(obj);
        }

        [HttpPatch("{userId}/password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto, [FromRoute] Guid userId)
        {
            var obj = await _service.ChangePassword(dto, userId);
            return Ok(obj);
        }

        [HttpDelete("{userId}/close")]
        public ActionResult Close(Guid userId)
        {
            var obj = _service.Close(userId);
            return Ok(obj);
        }
    }
}
