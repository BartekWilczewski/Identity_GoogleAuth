using Microsoft.AspNetCore.Identity;

namespace WebApplication4.Models
{
    public class MyIdentityUser : IdentityUser
    {
        public DateTime LastLoginAttempt { get; set; }
        public string FullName { get; set; }
    }
}
