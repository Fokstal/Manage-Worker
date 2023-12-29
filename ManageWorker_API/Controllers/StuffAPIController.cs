using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [AuthorizeExpiry]
    public class StuffAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<StuffDTO>> GetStuffs()
        {
            AppDbContext db = new();

            return Ok(db.Stuff);
        }

        [HttpGet("{id:int}", Name = "GetStuff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StuffDTO> GetStuff(int id)
        {
            if (id <= 0) return BadRequest();

            Stuff? stuff = new AppDbContext().Stuff.FirstOrDefault(stuff => stuff.Id == id);

            if (stuff is not null)
            {
                StuffDTO stuffDTO = new()
                {
                    Id = stuff.Id,
                    Name = stuff.Name,
                };

                return Ok(stuff);
            }


            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StuffDTO> CreateStuff([FromBody] StuffDTO stuffDTO)
        {
            // if (!ModelState.IsValid) return BadRequest(ModelState);
            if (new AppDbContext().Stuff.FirstOrDefault(stuff => stuff.Name.ToLower() == stuffDTO.Name.ToLower()) is not null)
            {
                ModelState.AddModelError("CustomError", "Stuff already Exists!");

                return BadRequest(ModelState);
            }

            if (stuffDTO is null) return BadRequest(stuffDTO);
            if (stuffDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            stuffDTO.Id = 1;

            using (AppDbContext db = new())
            {
                Stuff? stuffByIdLast = db.Stuff.OrderByDescending(stuff => stuff.Id).FirstOrDefault();

                if (stuffByIdLast is not null) stuffDTO.Id = stuffByIdLast.Id + 1;

                db.Stuff.Add(new()
                {
                    Id = stuffDTO.Id,
                    Name = stuffDTO.Name,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
            }

            return CreatedAtRoute("GetStuff", new { id = stuffDTO.Id }, stuffDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteStuff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteStuff(int id)
        {
            if (id <= 0) return BadRequest();

            using (AppDbContext db = new())
            {
                Stuff? stuff = db.Stuff.FirstOrDefault(stuff => stuff.Id == id);

                if (stuff is null) return NotFound();

                db.Stuff.Remove(stuff);

                db.SaveChanges();
            }

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateStuff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StuffDTO> UpdateStuff(int id, [FromBody] StuffDTO stuffDTO)
        {
            if (stuffDTO is null || id != stuffDTO.Id)
            {
                ModelState.AddModelError("CustomError", "Id in response and Id in Stuff is not equal!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {
                Stuff? stuffToUpdate = db.Stuff.FirstOrDefault(stuff => stuff.Id == id);

                if (stuffToUpdate is null) return NotFound();

                stuffToUpdate.Name = stuffDTO.Name;
                stuffToUpdate.CreatedDate = DateTime.Now;

                db.SaveChanges();
            }

            return NoContent();
        }
    }
}