using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageWorker_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerAPIController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Worker> GetWorkers()
        {
            return new List<Worker>
            {
                
            };
        }
    }
}