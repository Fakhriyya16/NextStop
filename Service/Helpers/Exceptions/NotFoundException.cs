
namespace Service.Helpers.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string keyword) : base($"{keyword} - was not found") { }
        
    }
}
