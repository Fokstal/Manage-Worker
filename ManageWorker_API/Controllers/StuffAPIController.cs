using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageWorker_API.Controllers
{
    [Route("stuff/")]
    [ApiController]
    [AuthorizeExpiry]
    public class StuffAPIController : ControllerBase
    {
        [HttpGet(Name = "GetStuffList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StuffDTO>>> GetStuffListAsync()
        {
            AppDbContext db = new();

            return Ok(await db.Stuff.ToArrayAsync());
        }

        [HttpGet("{id:int}", Name = "GetStuff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StuffDTO>> GetStuffAsync(int id)
        {
            if (id <= 0) return BadRequest();

            Stuff? stuff = await new AppDbContext().Stuff.FirstOrDefaultAsync(stuff => stuff.Id == id);

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
        public async Task<ActionResult<StuffDTO>> CreateStuffAsync([FromBody] StuffDTO stuffDTO)
        {
            if (await new AppDbContext().Stuff.FirstOrDefaultAsync(stuff => stuff.Name.ToLower() == stuffDTO.Name.ToLower()) is not null)
            {
                ModelState.AddModelError("CustomError", "Stuff already Exists!");

                return BadRequest(ModelState);
            }

            if (stuffDTO is null) return BadRequest(stuffDTO);
            if (stuffDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            using (AppDbContext db = new())
            {
                db.Stuff.Add(new()
                {
                    Name = stuffDTO.Name,
                    CreatedDate = DateTime.Now,
                });

                await db.SaveChangesAsync();
            }

            return CreatedAtRoute("GetStuff", new { id = stuffDTO.Id }, stuffDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteStuff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStuffAsync(int id)
        {
            if (id <= 0) return BadRequest();

            using (AppDbContext db = new())
            {
                Stuff? stuff = await db.Stuff.FirstOrDefaultAsync(stuff => stuff.Id == id);

                if (stuff is null) return NotFound();

                db.Stuff.Remove(stuff);

                await db.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateStuff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StuffDTO>> UpdateStuffAsync(int id, [FromBody] StuffDTO stuffDTO)
        {
            if (id < 1) return BadRequest();

            if (stuffDTO is null || id != stuffDTO.Id)
            {
                ModelState.AddModelError("CustomError", "Id and Id in model are not equals!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {
                Stuff? stuffToUpdate = await db.Stuff.FirstOrDefaultAsync(stuff => stuff.Id == id);

                if (stuffToUpdate is null) return NotFound();

                stuffToUpdate.Name = stuffDTO.Name;
                stuffToUpdate.CreatedDate = DateTime.Now;

                await db.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}