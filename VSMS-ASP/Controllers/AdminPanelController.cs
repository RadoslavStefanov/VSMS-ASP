using Microsoft.AspNetCore.Mvc;
using VSMS.Core.ViewModels;
using VSMS_ASP.Data;

namespace VSMS_ASP.Controllers
{
    public class AdminPanelController : Controller
    {
        private VSMS_ASPContext context;
        public AdminPanelController(VSMS_ASPContext _context)
        { context = _context; }
        public async Task<IActionResult> Show(AdminPanelViewModel model)
        {
            List<AllUsersListViewModel> users = new List<AllUsersListViewModel>();
            if (model.arg == "ListUsers")
            {
                users = context.Users.Select(x => new AllUsersListViewModel
                {
                    UserName = x.UserName,
                    Email = x.Email,
                    Role = "Not assigned!"
                }).ToList();
                View(users);
            }
            return View(users);
        }
    }
}
