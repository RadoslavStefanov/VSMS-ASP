using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;

namespace VSMS_ASP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsService productsService;
        public ProductsController(ProductsService _productsService)
        { productsService = _productsService; }

        public void ListProducts()
        {
            Response.Redirect("/Users/AdminPanel?arg=ListProducts");
        }

        public void Delete(string arg)
        {
            var user = context.Users.Where(x => x.Email == arg).First();
            context.Users.Remove(user);
            context.SaveChanges();
            Response.Redirect("/Users/AdminPanel?arg=ListUsers");
        }
    }
}
