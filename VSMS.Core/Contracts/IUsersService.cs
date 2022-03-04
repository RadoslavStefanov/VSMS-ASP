using VSMS.Core.ViewModels;

namespace VSMS.Core.Contracts
{
    public interface IUsersService
    {
        //bool CreateUser(RegisterViewModel model);
        string Login(LoginViewModel model);
        string GetUsername(string userId);
        (string userId, bool isCorrect) IsLoginCorrect(LoginViewModel model);
    }
}
