using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;

namespace VSMS_ASP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsService productsService;
        private readonly CategoriesService categoriesService;
        public ProductsController(ProductsService _productsService, CategoriesService _categoriesService)
        { 
            productsService = _productsService;
            categoriesService = _categoriesService;
        }

        public void ListProducts()
        {
            Response.Redirect("/AdminPanel/Show?arg=ListProducts");
        }

        public void Delete(string arg)
        {
            /*var user = context.Users.Where(x => x.Email == arg).First();
            context.Users.Remove(user);
            context.SaveChanges();
            Response.Redirect("/Users/AdminPanel?arg=ListUsers");*/
        }

        public async Task<IActionResult> CreateProduct()
        { 
            var list = categoriesService.GetAllCategories().ToList();
            list.Remove(list[0]);
            return await Task.Run(() => View(list)); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductsViewModel model)
        {
            await Task.Run(() => productsService.Create(model));
            return await Task.Run(() => Redirect("/AdminPanel/Show?arg=ListProducts"));
        }
    }
}
