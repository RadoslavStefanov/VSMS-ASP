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

        public (string userId, bool isCorrect) IsLoginCorrect(LoginViewModel model)
        {
            throw new NotImplementedException();
        }

        public string Login(LoginViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
