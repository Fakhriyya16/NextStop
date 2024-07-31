
namespace Service.DTOs.Accounts
{
    public class UserDetailDto
    {
        public int AppUserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Reviews { get; set; }
        public List<string> Blogs { get; set; }
        public List<string> Favorites { get; set; }
        public string SubscriptionType { get; set; }
        public IList<string> Roles { get; set; }
    }
}
