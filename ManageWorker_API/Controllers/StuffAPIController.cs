using ManageWorker_API.Data;
using ManageWorker_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StuffAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<StuffDTO>> GetStuffs()
        {
            return Ok(StuffStore.StuffList);
        }

        [HttpGet("{id:int}")]
        public ActionResult<StuffDTO> GetStuff(int id)
        {
            if (id <= 0) return BadRequest();

            StuffDTO? stuff = StuffStore.StuffList.FirstOrDefault(stuff => stuff.Id == id);

            if (stuff is not null) return Ok(stuff);

            return NotFound();
        }
    }
}