namespace ManageWorker_API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}