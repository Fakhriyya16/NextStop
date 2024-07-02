
using Domain.Common;

namespace Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
