using System.ComponentModel.DataAnnotations;

namespace ManageWorker_API.Models
{
    public class Stuff
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "N/N";

        public int CountWorker
        {
            get 
            {
                return WorkerList.Count;
            }
        }
        public List<Worker> WorkerList { get; set; } = [];
    }
}