using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageWorker_API.Models;
using ManageWorker_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ManageWorker_API.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class StuffAPIController
    {
        [HttpGet]
        public IEnumerable<StuffDTO> GetStuffs()
        {
            return new List<StuffDTO>
            {
                new() { Id = 1, Name = "BackEnd", CountWorker = 7 },
                new() { Id = 2, Name = "FrontEnd", CountWorker = 4 },
                new() { Id = 3, Name = "HR", CountWorker = 2 },
            };
        }
    }
}