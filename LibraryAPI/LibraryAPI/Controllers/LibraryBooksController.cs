using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;
using LibraryAPI.Models.Dto;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/")]
    public class LibraryBooksController : ControllerBase
    {
        private readonly ILibraryBooksService _service;

        public LibraryBooksController(ILibraryBooksService service)
        {
            _service = service;
        }

        [HttpGet("books")]
        public ActionResult Get()
        {
            var obj = _service.Get();
            return Ok(obj);
        }

        [HttpGet("book/{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            var obj = _service.Get(id);
            return Ok(obj);
        }

        [HttpPost("book")]
        public ActionResult Add([FromBody] BookDto dto)
        {
            var obj = _service.Add(dto);
            return Ok(obj);
        }

        [HttpPut("book/{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] BookDto dto)
        {
            var obj = _service.Update(id, dto);
            return Ok(obj);
        }

        [HttpPatch("book/{id}/quantity")]
        public ActionResult UpdateTotalQuantity([FromRoute] int id, [FromBody]  int quantity)
        {
            var obj = _service.UpdateTotalQuantity(id, quantity);
            return Ok(obj);
        }

        [HttpDelete("book/{id}")]
        public ActionResult Remove([FromRoute] int id)
        {
            var obj = _service.Remove(id);
            return Ok(obj);
        }
    }
}
