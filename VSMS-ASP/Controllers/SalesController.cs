using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;

namespace VSMS_ASP.Controllers
{
    public class SalesController : Controller
    {
        private readonly ProductsService productsService;
        private readonly CategoriesService categoriesService;

        public SalesController(ProductsService _productsService, CategoriesService _categoriesService)
        {productsService = _productsService;
        categoriesService = _categoriesService;}

        public IActionResult CashRegister()
        {
            var categoriesList = categoriesService.GetAllCategories();
            var productsList = productsService.GetAllProducts();
            var model = new List<AllProductsListViewModel>();

            foreach (var p in productsList)
            {
                model.Add(new AllProductsListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = categoriesList.Where(c => c.Id == p.CategoryId).FirstOrDefault().Name ?? "Unknown",
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Kilograms = p.Kilograms,
                    Price = p.Price
                });
            }
            ViewBag.Categories = categoriesList;
            return View(model);
        }

        [HttpPost]
        public void CashRegister(string saleJSON)
        {
            Console.WriteLine(saleJSON);
        }
    }
}
