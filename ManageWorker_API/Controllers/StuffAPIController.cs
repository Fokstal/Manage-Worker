using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageWorker_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{   
    [Route("api/StuffAPI")]
    [ApiController]
    public class StuffAPIController
    {
        [HttpGet]
        public IEnumerable<Stuff> GetStuffs()
        {
            return new List<Stuff>
            {
                new() { Id = 1, Name = "BackEnd", CountWorker = 7 },
                new() { Id = 2, Name = "FrontEnd", CountWorker = 4 },
                new() { Id = 3, Name = "HR", CountWorker = 2 },
            };
        }
    }
}