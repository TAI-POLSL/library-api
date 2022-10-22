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


        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            
            var obj = await _service.Register(dto);
            return Ok(obj);
        }

        [HttpPatch("{userId}")]
        public ActionResult Lock(Guid userId)
        {
            var obj = _service.Lock(userId);
            return Ok(obj);
        }

        [HttpPatch("password")]
        public ActionResult ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var obj = _service.ChangePassword(dto);
            return Ok(obj);
        }

        [HttpDelete("{userId}")]
        public ActionResult Close(Guid userId)
        {
            var obj = _service.Close(userId);
            return Ok(obj);
        }
    }
}
