using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManageWorker_API.Models
{
    public class Stuff
    {
        public int Id { get; set; }
        public string? Name { get; set; }  
        public int CountWorker { get; set; }  
    }
}