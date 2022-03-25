using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;

namespace VSMS_ASP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoriesService categoriesService;

        public CategoriesController(CategoriesService _categoriesService)
        {categoriesService = _categoriesService;}

        public void ListCategories()
        {Response.Redirect("/AdminPanel/Show?arg=ListCategories");}
        public void Create(string arg)
        { 
            categoriesService.Create(arg);
            Response.Redirect("/AdminPanel/Show?arg=ListCategories");
        }

        public void Delete(string arg)
        {
            categoriesService.Delete(arg);
            Response.Redirect("/AdminPanel/Show?arg=ListCategories");
        }
    }
}
