using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS_ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService usersService;
        private readonly Repository repo;
        public UsersController(UsersService _usersService,Repository _repo)
        { usersService = _usersService; repo = _repo; }

        public async Task<IActionResult> AdminPanel(AdminPanelViewModel model)
        {
            ViewData["Argument"] = model.arg;
            if (model.arg == "ListUsers")
            {
               var allUsers = repo.All<Users>().ToList();
            }
            return View(ViewData);
        }

        public void ListUsers()
        {
           Response.Redirect("/Users/AdminPanel?arg=ListUsers");
        }
    }
}
