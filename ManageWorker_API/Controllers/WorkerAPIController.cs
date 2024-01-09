using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ManageWorker_API.Service.WorkerAPIService;

namespace ManageWorker_API.Controllers
{
    [Route("worker/")]
    [ApiController]
    [AuthorizeExpiry]
    public class WorkerAPIController : ControllerBase
    {
        [HttpGet(Name = "GetWorkerList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkerListAsync()
        {
            AppDbContext db = new();

            List<Worker> workerList = [.. await db.Worker.ToArrayAsync()];

            return Ok(workerList);
        }

        [HttpGet("{id:int}", Name = "GetWorker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Worker>> GetWorkerAsync(int id)
        {
            if (id < 1) return BadRequest();

            AppDbContext db = new();

            Worker? worker = await db.Worker.FirstOrDefaultAsync(worker => worker.Id == id);

            if (worker is null) return NotFound();

            return Ok(worker);
        }

        [HttpPost(Name = "CreateWorker")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateWorkerAsync([FromForm] WorkerDTO workerDTO)
        {
            if (workerDTO is null) return BadRequest(workerDTO);
            if (workerDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            if (await new AppDbContext().Worker.FirstOrDefaultAsync(worker => worker.Name == workerDTO.Name && worker.StuffId == workerDTO.StuffId) is not null)
            {
                ModelState.AddModelError("CustomError", "Worker already exists!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {
                Stuff? stuff = await db.Stuff.FirstOrDefaultAsync(stuff => stuff.Id == workerDTO.StuffId);

                if (stuff is null) return NotFound();

                Worker worker = new()
                {
                    Name = workerDTO.Name,
                    StuffId = workerDTO.StuffId,
                    AvatarUrl = await UploadAvatarToFolderAsync(workerDTO.Avatar),
                };

                await db.Worker.AddAsync(worker);

                await db.SaveChangesAsync();

                return CreatedAtRoute("GetWorker", new { id = worker.Id }, worker);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteWorker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteWorkerAsync(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Worker? worker = await db.Worker.FirstOrDefaultAsync(worker => worker.Id == id);

                if (worker is null) return NotFound();

                if (worker.AvatarUrl != defaultAvatarName) System.IO.File.Delete($"{avatarFolderPath}{worker.AvatarUrl!}");

                db.Worker.Remove(worker);

                await db.SaveChangesAsync();

            }

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateWorker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateWorkerAsync(int id, [FromForm] WorkerDTO workerDTO)
        {
            if (id < 1) return BadRequest();

            if (workerDTO is null || id != workerDTO.Id)
            {
                ModelState.AddModelError("CustomError", "Id and Id in model are not equals!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {
                Worker? workerToUpdate = await db.Worker.FirstOrDefaultAsync(worker => worker.Id == id);

                if (workerToUpdate is null) return NotFound();

                Stuff? newStuff = await db.Stuff.FirstOrDefaultAsync(stuff => stuff.Id == workerDTO.StuffId);

                if (newStuff is null) return NotFound();

                if (workerToUpdate.AvatarUrl != defaultAvatarName) System.IO.File.Delete($"{avatarFolderPath}{workerToUpdate.AvatarUrl!}");

                workerToUpdate.Name = workerDTO.Name;
                workerToUpdate.AvatarUrl = await UploadAvatarToFolderAsync(workerDTO.Avatar);
                workerToUpdate.StuffId = workerDTO.StuffId;

                await db.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}