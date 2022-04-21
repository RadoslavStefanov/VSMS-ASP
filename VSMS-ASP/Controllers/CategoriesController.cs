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
            return View(await categoriesService.GetAllCategories());
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
