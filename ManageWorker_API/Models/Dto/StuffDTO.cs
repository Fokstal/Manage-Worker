using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManageWorker_API.Models.Dto
{
    public class StuffDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public int CountWorker { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}