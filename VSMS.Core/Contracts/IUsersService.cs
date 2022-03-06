using VSMS.Core.ViewModels;

namespace VSMS.Core.Contracts
{
    public interface IUsersService
    {
        //bool CreateUser(RegisterViewModel model);
        string LogIn(LoginViewModel model);
        string GetUsername(string userId);
        (string? tempData, bool isCorrect) IsLoginCorrect(LoginViewModel model);
        int getRolePower(string Username);
    }
}
