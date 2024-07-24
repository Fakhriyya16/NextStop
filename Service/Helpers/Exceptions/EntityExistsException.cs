
namespace Service.Helpers.Exceptions
{
    public class EntityExistsException : Exception
    {
        public EntityExistsException(string entity) : base($"{entity} - already exists.")
        {
            
        }
    }
}
