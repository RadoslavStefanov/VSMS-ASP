using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Mail;
using System.Text;
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


        [Authorize(Roles = "Admin,Employee,Guest")]
        public async Task<IActionResult> ListProducts()
        {
            ViewData["View"] = "Products";
            var products = await productsService.GetAllProducts();
            var list = await productsService.GetAllAsModelList();
            return await Task.Run(() => View(list.OrderBy(x => x.Category)));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await productsService.DeleteById(id))
            { return await Task.Run(() => Redirect("/Products/ListProducts")); }
            return await Task.Run(() => Redirect("/Error/CustomError?errorCode=202"));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct()
        {
            var list = (await categoriesService.GetAllCategories()).ToList();
            list.Remove(list[0]);
            return await Task.Run(() => View(list));
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(ProductsViewModel model)
        {
            await Task.Run(() => productsService.Create(model));
            return await Task.Run(() => Redirect("/Products/ListProducts"));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await productsService.GetProductByIdAsModel(id);
            if (product == null)
            { return await Task.Run(() => Redirect("/Error/CustomError?errorCode=500")); }

            ViewBag.Categories = (await categoriesService.GetAllCategories()).ToList();
            ViewBag.KilosList = new List<int>() { 1, 2, 3, 4, 5, 10, 15, 20, 25, 30, 35, 40, 45 };
            return await Task.Run(() => View(product));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(ProductsViewModel model)
        {
            await productsService.UpdateProduct(model);
            return await Task.Run(() => Redirect("/Products/ListProducts"));
        }


        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateOrder()
        { return await Task.Run(() => View()); }


        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateOrder(string? whatever)
        {
            //Does nothing for now
            return await Task.Run(() => Redirect("/Products/CreateOrder"));
        }


        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Quantities()
        {
            ViewBag.Products = await productsService.GetAllProducts();
            return await Task.Run(() => View());
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delivery()
        {
            var categoriesList = await categoriesService.GetAllCategories();
            ViewBag.Categories = categoriesList;
            var result = await productsService.GetAllAsModelList();
            return await Task.Run(() => View(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delivery(string deliveryJSON)
        {
            if (deliveryJSON == null || deliveryJSON.Length <= 0)
            { return await Task.Run(() => Redirect("/Products/Delivery")); }

            await productsService.RegisterDelivery(deliveryJSON);
            return await Task.Run(() => Redirect("/Products/Delivery"));
        }
    }
}
