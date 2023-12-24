using ManageWorker_API.Models.Dto;

namespace ManageWorker_API.Data
{
    public static class StuffStore
    {
        private static List<StuffDTO> stuffList =
            [
                new() { Id = 1, Name = "BackEnd", CountWorker = 7 },
                new() { Id = 2, Name = "FrontEnd", CountWorker = 4 },
                new() { Id = 3, Name = "HR", CountWorker = 2 },
            ];

        public static List<StuffDTO> StuffList { get => stuffList; set => stuffList = value; }
    }
}
