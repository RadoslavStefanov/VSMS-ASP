using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VSMS.Core.Contracts;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class UsersService : IUsersService
    {

        private readonly Repository repo;

        public UsersService(Repository _repo)
        { repo = _repo; }

        public string GetUsername(string userId)
        {
            throw new NotImplementedException();
        }

        public int getRolePower(string Username)
        {
            var power = repo.All<Users>()
                .Where(x => x.Username == Username)
                .Include(r => r.Role).Where(r=>r.RoleId==r.Role.Id)
                .Select(r=>r.Role.Power)
                .FirstOrDefault();
           return power;
        }

        public (string tempData, bool isCorrect) IsLoginCorrect(LoginViewModel model)
        {
            if (String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.UserName))
            { return ("Името или паролата не съвпадат с базата!", false); }
            else { return (null, true); }
        }

        public string LogIn(LoginViewModel model)
        {
            string passHash = CalculateHash(model.Password);

            var user = repo.All<Users>()
                 .Where(u => u.Username == model.UserName)
                 .Where(u => u.Password == CalculateHash(model.Password))
                 .SingleOrDefault();
            Console.WriteLine(user);

            return user?.Id;
        }

        private string CalculateHash(string password)
        {
            byte[] passworArray = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passworArray));
            }
        }
    }
}
