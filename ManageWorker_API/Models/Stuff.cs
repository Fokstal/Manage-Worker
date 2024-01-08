using System.ComponentModel.DataAnnotations.Schema;

namespace ManageWorker_API.Models
{
    public class Stuff
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        [NotMapped]
        public int CountWorker
        {
            get
            {
                return WorkerList.Count;
            }
        }
        
        [NotMapped]
        public List<Worker> WorkerList { get; set; } = [];

        public DateTime CreatedDate { get; set; }
    }
}