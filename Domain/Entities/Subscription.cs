
using Domain.Common;

namespace Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string SubscriptionType { get; set; }
    }
}
