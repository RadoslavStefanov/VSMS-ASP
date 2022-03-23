using Microsoft.AspNetCore.Identity;

namespace VSMS.Infrastructure.Migrations
{
    internal class PasswordHasher
    {
        private IdentityUser identityUser;

        public PasswordHasher(IdentityUser identityUser)
        {
            this.identityUser = identityUser;
        }
    }
}