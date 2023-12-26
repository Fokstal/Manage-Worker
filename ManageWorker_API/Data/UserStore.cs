using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageWorker_API.Data
{
    public static class UserStore
    {
        private static List<string> loginList =
        [
            "Admin321",
            "Tom3B"
        ];

        public static List<string> LoginList { get => loginList; set => loginList = value; }
    }
}