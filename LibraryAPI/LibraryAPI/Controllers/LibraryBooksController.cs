using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Interfaces;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/manage/")]
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
            var obj = _service.GetById(id);
            return Ok(obj);
        }

        [HttpPost("book")]
        public ActionResult Add()
        {
            var obj = _service.Add();
            return Ok(obj);
        }

        [HttpPut("book/{id}")]
        public ActionResult Update([FromRoute] int id)
        {
            var obj = _service.Update(id);
            return Ok(obj);
        }

        [HttpPatch("book/{id}")]
        public ActionResult UpdateTotalQuantity([FromRoute] int id)
        {
            var obj = _service.UpdateTotalQuantity(id);
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
