using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;

namespace VSMS_ASP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoriesService categoriesService;
        public CategoriesController(CategoriesService _categoriesService)
        {categoriesService = _categoriesService;}


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListCategories()
        {
            ViewData["View"] = "Categories";
            var categories = await categoriesService.GetAllCategories();
            var list = new List<CategoryViewModel>();
            foreach (var item in categories)
            {
                list.Add(new CategoryViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            list.Remove(list[0]);
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public async Task Create(string arg)
        { 
            await categoriesService.Create(arg);
            Response.Redirect("/Categories/ListCategories");
        }


        [Authorize(Roles = "Admin")]
        public async Task Delete(string arg)
        {
            await categoriesService.Delete(arg);
            Response.Redirect("/Categories/ListCategories");
        }
    }
}
