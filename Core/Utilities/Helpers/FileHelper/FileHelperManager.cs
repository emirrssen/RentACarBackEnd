using Core.Utilities.Helpers.GUIDHelper;
using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers.FileHelper
{
    public class FileHelperManager : IFileHelper
    {
        public void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string Update(IFormFile file, string filePath, string root)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Upload(file, root);
        }

        public string Upload(IFormFile file, string root)
        {
            if (file.Length > 0)
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                string extension = Path.GetExtension(file.FileName);
                string guid = GuidHelper.CreateGuid();
                string fileName = guid + extension;

                using (FileStream fileStream = File.Create(root + fileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    return fileName;
                }
            }
            return null;
        }
    }
}
