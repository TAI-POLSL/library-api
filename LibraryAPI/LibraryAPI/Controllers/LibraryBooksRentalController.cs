using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;

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

        [HttpGet("rental/{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            var obj = _service.GetById(id);
            return Ok(obj);
        }

        [HttpPost("rental")]
        public ActionResult Add()
        {
            var obj = _service.Add();
            return Ok(obj);
        }

        [HttpPut("rental/{id}")]
        public ActionResult Update([FromRoute] int id)
        {
            var obj = _service.Update(id);
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
