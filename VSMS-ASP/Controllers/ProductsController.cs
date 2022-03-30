using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
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

        public async Task<IActionResult> ListProducts()
        {
            dynamic myModel = new ExpandoObject();
            ViewData["View"] = "Products";
            var products = productsService.GetAllProducts();
            var list = new List<AllProductsListViewModel>();
            foreach (var item in products)
            {
                list.Add(new AllProductsListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = productsService.GetCategoryById(item.CategoryId),
                    ImageUrl = item.ImageUrl,
                    Description = item.Description,
                    Kilograms = item.Kilograms
                });
            }
            return await Task.Run(() => View(list));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (productsService.DeleteById(id))
            { return await Task.Run(() => Redirect("/Products/ListProducts")); }
            return await Task.Run(() => Redirect("/Error/CustomError?errorCode=202"));
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
            return await Task.Run(() => Redirect("/Products/ListProducts"));
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = productsService.GetAllProducts().Where(p=>p.Id==id).FirstOrDefault();
            if (product == null)
            {return await Task.Run(() => Redirect("/Error/CustomError?errorCode=500"));}

            var model = new ProductsViewModel()
            {
                Name = product.Name,
                Category = categoriesService.GetAllCategories().Where(c=>c.Id==product.CategoryId).FirstOrDefault().Name??"Unknown",
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Kilograms = $"{product.Kilograms}",
                Price = $"{product.Price}"
            };
            ViewBag.Categories = categoriesService.GetAllCategories().ToList();
            ViewBag.KilosList = new List<int>() { 5, 10, 15, 20, 25, 30, 35, 40, 45 };
            return await Task.Run(() => View(model));
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductsViewModel model)
        {
            return await Task.Run(() => Redirect("/Products/ListProducts"));
        }
    }
}
