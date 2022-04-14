using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using VSMS_ASP.Controllers;

namespace VSMS.Test.ControllerTests
{
    public class ContactsControllerTest
    {
        [Test]
        public void ShouldNotThrowWhenCalled()
        {
            var controller = new ContactsController();
            var result = controller.Show() as ViewResult;
            Assert.AreEqual("~/Views/Contacts/Show.cshtml", result.ViewName);
        }
    }
}
