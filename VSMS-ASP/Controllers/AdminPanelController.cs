using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;
using VSMS_ASP.Data;

namespace VSMS_ASP.Controllers
{
    public class AdminPanelController : Controller
    {
        private VSMS_ASPContext context;
        private readonly ProductsService productsService;
        public AdminPanelController(VSMS_ASPContext _context, ProductsService _productsService)
        { 
            context = _context;
            productsService = _productsService;
        }


        public async Task<IActionResult> Show(AdminPanelViewModel model)
        {
            dynamic myModel = new ExpandoObject();

            if (model.arg == "ListUsers")
            {
                List<AllUsersListViewModel> users = new List<AllUsersListViewModel>();
                ViewData["View"]= "Users";
                users = context.Users.Select(x => new AllUsersListViewModel
                {
                    UserName = x.UserName,
                    Email = x.Email,
                    Role = 
                    context.Roles
                    .Where(y => y.Id == context.UserRoles
                    .Where(i => i.UserId == x.Id)
                    .FirstOrDefault().RoleId)
                    .FirstOrDefault().Name ?? "Not Configured/Limited"
                }).ToList();
                myModel.Users = users;
                return View(myModel);
            }
            if (model.arg == "ListProducts")
            {
                ViewData["View"] = "Products";
                var products = productsService.GetAllProducts();
                var list = new List<AllProductsListViewModel>();
                foreach (var item in products)
                {
                    list.Add(new AllProductsListViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        CategoryId = item.CategoryId,
                        ImageUrl = item.ImageUrl
                    });
                }
                myModel.Products = list;
                return View(myModel);
            }
            return View(null);
        }

        

    }
}
