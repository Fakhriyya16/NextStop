
namespace Service.Helpers.Exceptions
{
    public class FileSizeExceededException : Exception
    {
        public FileSizeExceededException() : base("The file size exceeds the allowed limit.")
        {
        }

        public FileSizeExceededException(string message) : base(message)
        {
        }
    }
}
