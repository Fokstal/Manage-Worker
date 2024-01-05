namespace ManageWorker_API.Service
{
    public static class WorkerAPIService
    {
        public static readonly string avatarFolderPath = "./AppData/images/avatars/";

        public static async Task<string> UploadAvatarToFolderAsync(IFormFile? avatar)
        {
            string avatarUrl = "default.jpg";

            if (avatar is not null)
            {
                string avatarGuidName = Guid.NewGuid().ToString();
                string avatarExtension = Path.GetExtension(avatar.FileName);
                using FileStream fileStream = new(Path.Combine(avatarFolderPath, avatarGuidName + avatarExtension), FileMode.Create);

                await avatar.CopyToAsync(fileStream);

                avatarUrl = avatarGuidName + avatarExtension;
            }
            return avatarUrl;
        }
    }
}