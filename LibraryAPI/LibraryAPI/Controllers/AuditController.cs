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
        private readonly IHeaderContextService _headerContextService;

        public AuditController(
            IAuditService service,
            IHeaderContextService headerContextService
        ) {
            _service = service;
            _headerContextService = headerContextService;
        }

        [HttpGet("audits/db")]
        [Authorize(Roles = "ADMIN")]
        public object GetAudits()
        {
            var obj = _service.GetAudits();
            return Ok(obj);
        }

        [HttpGet("security")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult GetSecurity()
        {
            var obj = _service.GetSecurity();
            return Ok(obj);
        }

        [HttpGet("security/{userId}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult GetSecurityByUserId([FromRoute] Guid userId)
        {
            var obj = _service.GetSecurityByUserId(userId);
            return Ok(obj);
        }

        [HttpGet("security/own")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public ActionResult GetOwnSecurityAudits()
        {
            Guid userId = _headerContextService.GetUserId();
            var obj = _service.GetSecurityByUserId(userId);
            return Ok(obj);
        }

        [HttpGet("sessions/{userId}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult GetUserSessions([FromRoute] Guid userId)
        {
            var obj = _service.GetUserSessions(userId);
            return Ok(obj);
        }

        [HttpGet("sessions/own")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public ActionResult GetOwnSessions()
        {
            Guid userId = _headerContextService.GetUserId();
            var obj = _service.GetUserSessions(userId);
            return Ok(obj);
        }
    }
}
