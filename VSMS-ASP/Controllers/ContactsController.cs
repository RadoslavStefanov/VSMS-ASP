using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Show()
        {return View("~/Views/Contacts/Show.cshtml");}
    }
}
