using ManageWorker_API.Data;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeExpiry]
    public class StuffAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<StuffDTO>> GetStuffs() => Ok(StuffStore.StuffList);

        [HttpGet("{id:int}", Name = "GetStuff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StuffDTO> GetStuff(int id)
        {
            if (id <= 0) return BadRequest();

            StuffDTO? stuff = StuffStore.StuffList.FirstOrDefault(stuff => stuff.Id == id);

            if (stuff is not null) return Ok(stuff);

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StuffDTO> CreateStuff([FromBody] StuffDTO stuffDTO)
        {
            // if (!ModelState.IsValid) return BadRequest(ModelState);
            if (StuffStore.StuffList.FirstOrDefault(stuff => stuff.Name.ToLower() == stuffDTO.Name.ToLower()) is not null)
            {
                ModelState.AddModelError("CustomError", "Stuff already Exists!");

                return BadRequest(ModelState);
            }

            if (stuffDTO is null) return BadRequest(stuffDTO);
            if (stuffDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            stuffDTO.Id = 1;

            StuffDTO? stuffByIdLast = StuffStore.StuffList.OrderByDescending(stuff => stuff.Id).FirstOrDefault();

            if (stuffByIdLast is not null) stuffDTO.Id = stuffByIdLast.Id + 1;

            StuffStore.StuffList.Add(stuffDTO);

            return CreatedAtRoute("GetStuff", new { id = stuffDTO.Id }, stuffDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteStuff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteStuff(int id)
        {
            if (id <= 0) return BadRequest();

            StuffDTO? stuff = StuffStore.StuffList.FirstOrDefault(stuff => stuff.Id == id);

            if (stuff is null) return NotFound();

            StuffStore.StuffList.Remove(stuff);

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateStuff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StuffDTO> UpdateStuff(int id, [FromBody] StuffDTO stuffDTO)
        {
            if (stuffDTO is null || id != stuffDTO.Id) return BadRequest();

            StuffDTO? stuffToUpdate = StuffStore.StuffList.FirstOrDefault(stuff => stuff.Id == id);

            if (stuffToUpdate is null) return NotFound();

            stuffToUpdate.Name = stuffDTO.Name;
            stuffToUpdate.CountWorker = stuffDTO.CountWorker;

            return NoContent();
        }
    }
}