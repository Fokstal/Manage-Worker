using System.ComponentModel.DataAnnotations;

namespace ManageWorker_API.Models.Dto
{
    public class StuffDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "N/N";
    }
}