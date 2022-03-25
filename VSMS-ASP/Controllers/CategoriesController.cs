using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class CategoriesController : Controller
    {
        public void ListCategories()
        {Response.Redirect("/AdminPanel/Show?arg=ListCategories");}
    }
}
