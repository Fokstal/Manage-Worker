using System.ComponentModel.DataAnnotations.Schema;

namespace ManageWorker_API.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }

        // [NotMapped]
        // public IFormFile? Avatar { get; set; }

        public int StuffId { get; set; }

        [ForeignKey(nameof(StuffId))]
        public Stuff? Stuff { get; set; } 
    }
}