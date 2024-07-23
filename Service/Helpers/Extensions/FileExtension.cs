using Microsoft.AspNetCore.Http;
using Service.Helpers.Exceptions;


namespace Service.Helpers.Extensions
{
    public static class FileExtension
    {
        public static bool IsImage(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (!file.ContentType.Contains("image"))
                return false;

            return true;
        }

        public static bool IsValidSize(this IFormFile file, long maxFileSize)
        {
            return file.Length / 1024 < maxFileSize;
        }
    }
}
