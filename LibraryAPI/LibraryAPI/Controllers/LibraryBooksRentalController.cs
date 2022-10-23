using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/manage/")]
    public class LibraryBooksRentalController : ControllerBase
    {
        private readonly ILibraryBooksRentalService _service;

        public LibraryBooksRentalController(ILibraryBooksRentalService service)
        {
            _service = service;
        }

        [HttpGet("rentals")]
        public ActionResult Get()
        {
            var obj = _service.Get();
            return Ok(obj);
        }

        [HttpGet("rental/{id}/details")]
        public ActionResult GetById([FromRoute] int id)
        {
            var obj = _service.Get(id);
            return Ok(obj);
        }

        [HttpGet("rentals/user/{userId}")]
        public ActionResult GetByUser([FromRoute] Guid userId)
        {
            var obj = _service.Get(null, userId);
            return Ok(obj);
        }

        [HttpPost("rental")]
        public ActionResult Add([FromBody] BookReservationDto dto)
        {
            var obj = _service.Add(dto);
            return Ok(obj);
        }

        [HttpPut("rental/{id}/cancel")]
        public ActionResult Cancel([FromRoute] int id)
        {
            var obj = _service.Cancel(id);
            return Ok(obj);
        }

        [HttpPatch("rental/{id}/end")]
        public ActionResult End([FromRoute] int id)
        {
            var obj = _service.End(id);
            return Ok(obj);
        }
    }
}
