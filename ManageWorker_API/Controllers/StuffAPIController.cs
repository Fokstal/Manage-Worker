using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageWorker_API.Data;
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
            return StuffStore.StuffList;
        }
    }
}