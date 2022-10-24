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

        [HttpGet("db/audits")]
        [Authorize(Roles = "ADMIN")]
        public object GetAudits()
        {
            var obj = _service.GetAudits();
            return Ok(obj);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public ActionResult GetSecurity()
        {
            var obj = _service.GetSecurity();
            return Ok(obj);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public ActionResult GetSecurityByUserId([FromRoute] Guid userId)
        {
            // TODO EMPLOYEE, CLIENT get only own
            var obj = _service.GetSecurityByUserId(userId);
            return Ok(obj);
        }

        [HttpGet("{userId}/sessions")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public ActionResult GetUserSessions([FromRoute] Guid userId)
        {
            // TODO EMPLOYEE, CLIENT get only own
            var obj = _service.GetUserSessions(userId);
            return Ok(obj);
        }
    }
}
