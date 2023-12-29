
using System.ComponentModel.DataAnnotations;

namespace ManageWorker_API.Models.Dto
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(15)]
        public string Login { get; set; } = null!;

        [MaxLength(25)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; } = null!;
    }
}