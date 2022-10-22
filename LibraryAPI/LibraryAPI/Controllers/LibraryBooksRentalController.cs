using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("/api/1.0.0/library/manage/")]
    public class LibraryBooksRentalController : ControllerBase
    {
        public LibraryBooksRentalController()
        {
        }

        [HttpGet("rentals")]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpGet("rental/{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpPost("rental")]
        public ActionResult Add()
        {
            return Ok();
        }

        [HttpPut("rental/{id}")]
        public ActionResult Update([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpPatch("rental/{id}/end")]
        public ActionResult End([FromRoute] int id)
        {
            return Ok(id);
        }
    }
}
