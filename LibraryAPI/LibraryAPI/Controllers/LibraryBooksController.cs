using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/manage/")]
    public class LibraryBooksController : ControllerBase
    {
        public LibraryBooksController()
        {
        }

        [HttpGet("books")]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpGet("book/{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpPost("book")]
        public ActionResult Add()
        {
            return Ok();
        }

        [HttpPut("book/{id}")]
        public ActionResult Update([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpPatch("book/{id}")]
        public ActionResult UpdateTotalQuantity([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpDelete("book/{id}")]
        public ActionResult Remove([FromRoute] int id)
        {
            return Ok(id);
        }
    }
}
