using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApplication4.Models;

namespace WebApplication4
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<MyIdentityUser>
    {
        public MyUserClaimsPrincipalFactory(UserManager<MyIdentityUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(MyIdentityUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(nameof(user.FullName), user.FullName));
            return identity;
        }
    }
}
