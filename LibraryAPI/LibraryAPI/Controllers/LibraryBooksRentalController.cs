using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/")]
    public class LibraryBooksRentalController : ControllerBase
    {
        private readonly ILibraryBooksRentalService _service;

        public LibraryBooksRentalController(ILibraryBooksRentalService service)
        {
            _service = service;
        }

        [HttpGet("rentals")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public ActionResult Get()
        {
            // CLIENT can get only own rentals
            var obj = _service.Get();
            return Ok(obj);
        }

        [HttpGet("rental/{id}/info")]
        [Authorize(Roles = "ADMIN, EMPLOYEE, CLIENT")]
        public ActionResult GetById([FromRoute] int id)
        {
            // CLIENT can get only own rental details
            var obj = _service.Get(id);
            return Ok(obj);
        }

        [HttpGet("rentals/user/{userId}")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult GetByUser([FromRoute] Guid userId)
        {
            var obj = _service.Get(null, userId);
            return Ok(obj);
        }

        [HttpPost("rental")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult Add([FromBody] BookRentByUserDto dto)
        {
            // Only clients can rents books
            var obj = _service.Add(dto);
            return Ok(obj);
        }

        [HttpPut("rental/{id}/cancel")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult Cancel([FromRoute] int id)
        {
            var obj = _service.Cancel(id);
            return Ok(obj);
        }

        [HttpPatch("rental/{id}/end")]
        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        public ActionResult End([FromRoute] int id)
        {
            var obj = _service.End(id);
            return Ok(obj);
        }
    }
}
