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
        private readonly MailService mailService;
        private UserManager<IdentityUser> userManager;

        public SalesController(ProductsService _productsService,
               CategoriesService _categoriesService,
               SalesService _salesService,
               MailService _mailService,
               UserManager<IdentityUser> usermgr)
        {
            productsService = _productsService;
            categoriesService = _categoriesService;
            salesService = _salesService;
            userManager = usermgr;
            mailService = _mailService;
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
            ViewBag.Total = salesService.GetTodayIncomeForUser(userManager.GetUserId(User));
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

            ViewBag.RestrictedView = true;
            ViewBag.Total = 0;
            foreach (var item in mySales)
            { ViewBag.Total += item.TotalPrice; }
            return View(mySales);
        }

        [HttpPost]
        public async Task<IActionResult> MySales(string quantityJSON)
        {
            mailService.SendSalesReport(quantityJSON);
            return Redirect("/Sales/MySales");
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
            ViewBag.RestrictedView = false;
            ViewBag.Date = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
            return View("MySales", sales);
        }

        [HttpPost]
        public async Task<IActionResult> AllSales(string quantityJSON)
        {
            mailService.SendSalesReport(quantityJSON);
            return Redirect("/Sales/AllSales");
        }
    }
}
