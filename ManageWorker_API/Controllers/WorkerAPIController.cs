using ManageWorker_API.Data;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using ManageWorker_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Ctrl + G + {numberStroke} - go to stroke by number
// Shirt + [ArrUp] / [ArrDown] - copy stroke all
// Ctrl + Shift + \ - go to {} (skobki in class,)

namespace ManageWorker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [AuthorizeExpiry]
    public class WorkerAPIController : ControllerBase
    {   
        private static readonly string avatarFolderPath = "./AppData/images/avatars/";

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

                return Created();
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

            if (id != workerDTO.Id)
            {
                ModelState.AddModelError("CustomError", "Id and Id in model are not equals!");
            }

            using (AppDbContext db = new())
            {
                Worker? worker = db.Worker.FirstOrDefault(worker => worker.Id == id);

                if (worker is null) return NotFound();

                Stuff? newStuff = db.Stuff.FirstOrDefault(stuff => stuff.Id == workerDTO.StuffId);

                if (newStuff is null) return NotFound();

                worker.Name = workerDTO.Name;
                worker.AvatarUrl = UploadAvatarToFolder(workerDTO.Avatar);
                worker.StuffId = workerDTO.StuffId;
                worker.Stuff = newStuff;

                db.SaveChanges();
            }

            return NoContent();
        }

        private static string UploadAvatarToFolder(IFormFile? avatar)
        {
            string avatarUrl = "default.jpg";

            if (avatar is not null)
            {
                string avatarGuidName = Guid.NewGuid().ToString();
                string avatarExtension = Path.GetExtension(avatar.FileName);
                using FileStream fileStream = new(Path.Combine(avatarFolderPath, avatarGuidName + avatarExtension), FileMode.Create);

                avatar.CopyTo(fileStream);

                avatarUrl = avatarGuidName + avatarExtension;
            }
            return avatarUrl;
        }
    }
}