using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StuffAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<StuffDTO>> GetStuffs() => Ok(StuffStore.StuffList);

        [HttpGet("{id:int}")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StuffDTO> CreateStuff([FromBody]StuffDTO stuff)
        {
            if (stuff is null) return BadRequest(stuff);
            if (stuff.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            stuff.Id = StuffStore.StuffList.OrderByDescending(stuff => stuff.Id).FirstOrDefault().Id + 1;

            StuffStore.StuffList.Add(stuff);

            return Ok(stuff);
        }
    }
}