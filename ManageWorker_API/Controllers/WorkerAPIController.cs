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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Worker>> GetWorkers()
        {
            AppDbContext db = new();

            List<Worker> workerList = [.. db.Worker.Include(worker => worker.Stuff)];

            workerList.ForEach(worker =>
            {
                using FileStream fileStream = System.IO.File.OpenRead(avatarFolderPath + worker.AvatarUrl!);

                worker.Avatar = new FormFile(fileStream, 0, fileStream.Length, null!, Path.GetFileName(fileStream.Name));
            });

            return Ok(workerList);
        }

        [HttpGet("{id:int}", Name = "GetWorker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Worker> GetWorker(int id)
        {
            if (id < 1) return BadRequest();

            AppDbContext db = new();

            Worker? worker = db.Worker.FirstOrDefault(worker => worker.Id == id);

            if (worker is null) return NotFound();

            using FileStream fileStream = System.IO.File.OpenRead(avatarFolderPath + worker.AvatarUrl!);

            worker.Avatar = new FormFile(fileStream, 0, fileStream.Length, null!, Path.GetFileName(fileStream.Name));

            return Ok(worker);
        }

        [HttpPost(Name = "CreateWorker")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateWorker([FromForm] WorkerDTO workerDTO)
        {
            if (workerDTO is null) return BadRequest(workerDTO);
            if (workerDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            if (new AppDbContext().Worker.FirstOrDefault(worker => worker.Name == workerDTO.Name && worker.StuffId == workerDTO.StuffId) is not null)
            {
                ModelState.AddModelError("CustomError", "Worker already exists!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {
                Stuff? stuff = db.Stuff.FirstOrDefault(stuff => stuff.Id == workerDTO.StuffId);

                if (stuff is null) return NotFound();

                Worker worker = new()
                {
                    Name = workerDTO.Name,
                    StuffId = workerDTO.StuffId,
                    Stuff = stuff,
                    AvatarUrl = UploadAvatarToFolder(workerDTO.Avatar),
                };

                db.Worker.Add(worker);

                db.SaveChanges();

                return CreatedAtRoute("GetWorker", new { id = worker.Id }, worker);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteWorker")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteWorker(int id)
        {
            if (id < 1) return BadRequest();

            using (AppDbContext db = new())
            {
                Worker? worker = db.Worker.FirstOrDefault(worker => worker.Id == id);

                if (worker is null) return NotFound();

                db.Worker.Remove(worker);

                db.SaveChanges();

                System.IO.File.Delete(worker.AvatarUrl!);
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
        public ActionResult UpdateWorker(int id, [FromBody] WorkerDTO workerDTO)
        {
            if (id < 1) return BadRequest();

            if (workerDTO is null || id != workerDTO.Id)
            {
                ModelState.AddModelError("CustomError", "Id and Id in model are not equals!");

                return BadRequest(ModelState);
            }

            using (AppDbContext db = new())
            {
                Worker? workerToUpdate = db.Worker.FirstOrDefault(worker => worker.Id == id);

                if (workerToUpdate is null) return NotFound();

                Stuff? newStuff = db.Stuff.FirstOrDefault(stuff => stuff.Id == workerDTO.StuffId);

                if (newStuff is null) return NotFound();

                workerToUpdate.Name = workerDTO.Name;
                workerToUpdate.AvatarUrl = UploadAvatarToFolder(workerDTO.Avatar);
                workerToUpdate.StuffId = workerDTO.StuffId;
                workerToUpdate.Stuff = newStuff;

                db.SaveChanges();
            }

            return NoContent();
        }
    }
}