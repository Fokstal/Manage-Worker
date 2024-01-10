using System.ComponentModel.DataAnnotations;

namespace ManageWorker_API.Models.Dto
{
    public class WorkerDTO
    {
        public int Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string? Name { get; set; }
        public IFormFile? Avatar { get; set; }

        [Required]
        public int StuffId { get; set; }
    }
}