
using Domain.Common;

namespace Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string SubscriptionType { get; set; }
    }
}
