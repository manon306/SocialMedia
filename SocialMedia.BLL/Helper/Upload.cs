namespace SocialMedia.BLL.Helper
{
    public class Upload
    {
        public static List<string> UploadFile(string FolderName, List<IFormFile> Files)
        {
            var savedFiles = new List<string>();

            try
            {
                // 1) Get Directory
                string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);

                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                // 2) Loop through files
                foreach (var file in Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        // unique file name
                        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                        // final path
                        string finalPath = Path.Combine(FolderPath, fileName);

                        // save file
                        using (var stream = new FileStream(finalPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // relative path (علشان تخزني في الداتابيز)
                        savedFiles.Add(Path.Combine(FolderName, fileName));
                    }
                }

                return savedFiles;
            }
            catch (Exception ex)
            {
                // ممكن ترجع List فاضية أو ترمي exception
                throw new Exception("Error while uploading files: " + ex.Message);
            }
        }

        public static string RemoveFile(string FolderName, string fileName)
        {
            try
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName, fileName);

                if (File.Exists(directory))
                {
                    File.Delete(directory);
                    return "File Deleted";
                }

                return "File Not Deleted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
