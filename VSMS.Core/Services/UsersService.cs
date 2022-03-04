using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSMS.Core.Contracts;
using VSMS.Core.ViewModels;

namespace VSMS.Core.Services
{
    public class UsersService : IUsersService
    {
        public string GetUsername(string userId)
        {
            throw new NotImplementedException();
        }

        public (string tempData, bool isCorrect) IsLoginCorrect(LoginViewModel model)
        {
            if (String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.UserName))
            { return ("Името или паролата не съвпадат с базата!", false); }
            else { return (null, true); }
        }

        public (string error, bool isLogged) LogIn(LoginViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
