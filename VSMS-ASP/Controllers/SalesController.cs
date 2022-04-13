using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace VSMS_ASP.Controllers
{
    public class SalesController : Controller
    {
        private readonly ProductsService productsService;
        private readonly CategoriesService categoriesService;
        private readonly SalesService salesService;
        private UserManager<IdentityUser> userManager;

        public SalesController(ProductsService _productsService,
               CategoriesService _categoriesService,
               SalesService _salesService,
               UserManager<IdentityUser> usermgr)
        {
            productsService = _productsService;
            categoriesService = _categoriesService;
            salesService = _salesService;
            userManager = usermgr;
        }


        [Authorize(Roles = "Admin,Employee,Guest")]
        public async Task<IActionResult> CashRegister()
        {
            var categoriesList = await categoriesService.GetAllCategories();
            var productsList = await productsService.GetAllProducts();
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
            return View(model.OrderByDescending(p => p.Category));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CashRegister(string saleJSON)
        {
            if (saleJSON == null)
            { return Redirect("/Sales/CashRegister"); }
            if (saleJSON.Length <= 0)
            { return Redirect("/Sales/CashRegister"); }

            var un = userManager.GetUserId(User);
            salesService.RegisterSale(saleJSON, un);
            return Redirect("/Sales/CashRegister");
        }

        [Authorize(Roles = "Admin,Employee,Guest")]
        public async Task<IActionResult> MySales()
        {
            var userId = userManager.GetUserId(User);
            var mySales = (await salesService.GetUserSales(userId, User.Identity.Name)).OrderBy(p => p.DateTime);
            ViewBag.Date = $"{DateTime.Now.ToString("yyyy-MM-dd")}";

            ViewBag.Total = 0;
            foreach (var item in mySales)
            { ViewBag.Total += item.TotalPrice; }
            return View(mySales);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllSales()
        {
            var sales = await salesService.GetSales();
            foreach (var item in sales)
            {
                var curUserId = item.Seller;
                var user = await userManager.FindByIdAsync(curUserId);
                item.Seller = user.Email;
            }
            ViewBag.Date = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
            return View("MySales", sales);
        }
    }
}
