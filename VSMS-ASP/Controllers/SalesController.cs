﻿using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

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
        public IActionResult CashRegister(string saleJSON)
        {
            if (saleJSON == null)
            { return Redirect("/Sales/CashRegister"); }
            if (saleJSON.Length <= 0)
            { return Redirect("/Sales/CashRegister"); }

            var un = userManager.GetUserId(User);
            salesService.RegisterSale(saleJSON,un);
            return Redirect("/Sales/CashRegister");
        }

        public IActionResult MySales()
        {
            var userId = userManager.GetUserId(User);
            var mySales = salesService.GetUserSales(userId,User.Identity.Name).OrderBy(p=>p.DateTime);
            ViewBag.Date = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
            return View(mySales);
        }

        public async Task<IActionResult> AllSales()
        {
            var sales = salesService.GetSales();
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
