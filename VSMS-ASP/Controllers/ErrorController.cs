using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Constants;

namespace VSMS_ASP.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View();
        }
        
        public IActionResult customError(int errorCode)
        {
            var erros = new ErrorMessages();
            var message = erros.getMessage(errorCode);
            if (message == null)
            {ViewBag.msg = "Unknown error with code: " + errorCode;}
            else
            {ViewBag.msg = message.ToString();}

            return View();
        }
    }
}
