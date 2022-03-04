

namespace VSMS.Core.Contracts
{
    public interface IUserService
    {
        //bool Register(RegisterViewModel model);
        //string Login(LoginViewModel model);
        string GetUsername(string userId);
        //(string userId, bool isCorrect) IsLoginCorrect(LoginViewModel model);
    }
}
