namespace ManageWorker_API.Service
{
    public static class WorkerAPIService
    {
        public static readonly string avatarFolderPath = "./wwwroot/images/avatars/";
        public static readonly string defaultAvatarName = "default.jpg";

        public static async Task<string> UploadAvatarToFolderAsync(IFormFile? avatar)
        {
            string defaultAvatarUrl = defaultAvatarName;

            if (avatar is not null)
            {
                string avatarGuidName = Guid.NewGuid().ToString();
                string avatarExtension = Path.GetExtension(avatar.FileName);
                using FileStream fileStream = new(Path.Combine(
                    avatarFolderPath,
                    avatarGuidName + avatarExtension), FileMode.Create);

                await avatar.CopyToAsync(fileStream);

                defaultAvatarUrl = avatarGuidName + avatarExtension;
            }

            return defaultAvatarUrl;
        }
    }
}