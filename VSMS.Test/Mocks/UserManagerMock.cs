using Microsoft.AspNetCore.Identity;
using Moq;

namespace VSMS.Test.Mocks
{
    public class UserManagerMock
    {
        public static UserManager<IdentityUser> Instance
        {
            get
            {
                var store = new Mock<IUserStore<IdentityUser>>();
                var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
                mgr.Object.UserValidators.Add(new UserValidator<IdentityUser>());
                mgr.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

                mgr.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);
                mgr.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);

                return mgr.Object;
            }
        }
    }
}