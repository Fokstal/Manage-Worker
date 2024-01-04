namespace ManageWorker_API.Service
{
    public static class WorkerAPIService
    {
        public static readonly string avatarFolderPath = "./AppData/images/avatars/";

        public static string UploadAvatarToFolder(IFormFile? avatar)
        {
            string avatarUrl = "default.jpg";

            if (avatar is not null)
            {
                string avatarGuidName = Guid.NewGuid().ToString();
                string avatarExtension = Path.GetExtension(avatar.FileName);
                using FileStream fileStream = new(Path.Combine(avatarFolderPath, avatarGuidName + avatarExtension), FileMode.Create);

                avatar.CopyTo(fileStream);

                avatarUrl = avatarGuidName + avatarExtension;
            }
            return avatarUrl;
        }
    }
}