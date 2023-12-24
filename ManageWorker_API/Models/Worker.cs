using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ManageWorker_API.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }

        public int StuffId { get; set; }

        [ForeignKey(nameof(StuffId))]
        public Stuff? Stuff { get; set; } 
    }
}