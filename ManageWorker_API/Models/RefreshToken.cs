using System.ComponentModel.DataAnnotations.Schema;

namespace ManageWorker_API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public DateTime ExpiryTime { get; set; }
        
        
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}