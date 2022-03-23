using Microsoft.AspNetCore.Mvc;
using VSMS.Core.ViewModels;
using VSMS_ASP.Data;

namespace VSMS_ASP.Controllers
{
    public class UsersController : Controller
    {
        private VSMS_ASPContext context;
        public UsersController(VSMS_ASPContext _context)
        {context = _context;}


        public void ListUsers()
        {
           Response.Redirect("/AdminPanel/Show?arg=ListUsers");
        }

        public void Delete(string arg)
        {
            var user = context.Users.Where(x => x.Email == arg).First();
            context.Users.Remove(user);
            context.SaveChanges();
            Response.Redirect("/AdminPanel/Show?arg=ListUsers");
        }
    }
}
