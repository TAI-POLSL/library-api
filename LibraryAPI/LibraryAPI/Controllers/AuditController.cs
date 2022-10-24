using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/logs/")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _service;

        public AuditController(
            IAuditService service
        ) {
            _service = service;
        }

        [HttpGet]
        public ActionResult GetSecurity()
        {
            var obj = _service.GetSecurity();
            return Ok(obj);
        }

        [HttpGet("{userId}")]
        public ActionResult GetSecurityByUserId([FromRoute] Guid userId)
        {
            var obj = _service.GetSecurityByUserId(userId);
            return Ok(obj);
        }

        [HttpGet("{userId}/sessions")]
        public ActionResult GetUserSessions([FromRoute] Guid userId)
        {
            var obj = _service.GetUserSessions(userId);
            return Ok(obj);
        }
    }
}
