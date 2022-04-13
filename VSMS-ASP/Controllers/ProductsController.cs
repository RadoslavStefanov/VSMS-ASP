using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Mail;
using System.Text;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;

namespace VSMS_ASP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsService productsService;
        private readonly CategoriesService categoriesService;
        public ProductsController(ProductsService _productsService, CategoriesService _categoriesService)
        {
            productsService = _productsService;
            categoriesService = _categoriesService;
        }


        [Authorize(Roles = "Admin,Employee,Guest")]
        public async Task<IActionResult> ListProducts()
        {
            dynamic myModel = new ExpandoObject();
            ViewData["View"] = "Products";
            var products = await productsService.GetAllProducts();
            var list = new List<AllProductsListViewModel>();
            foreach (var item in products)
            {
                list.Add(new AllProductsListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = await productsService.GetCategoryById(item.CategoryId),
                    ImageUrl = item.ImageUrl,
                    Description = item.Description,
                    Kilograms = item.Kilograms
                });
            }
            return await Task.Run(() => View(list.OrderBy(x => x.Category)));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await productsService.DeleteById(id))
            { return await Task.Run(() => Redirect("/Products/ListProducts")); }
            return await Task.Run(() => Redirect("/Error/CustomError?errorCode=202"));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct()
        {
            var list = (await categoriesService.GetAllCategories()).ToList();
            list.Remove(list[0]);
            return await Task.Run(() => View(list));
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(ProductsViewModel model)
        {
            await Task.Run(() => productsService.Create(model));
            return await Task.Run(() => Redirect("/Products/ListProducts"));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = (await productsService.GetAllProducts()).Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            { return await Task.Run(() => Redirect("/Error/CustomError?errorCode=500")); }

            var model = new ProductsViewModel()
            {
                Name = product.Name,
                Category = (await categoriesService.GetAllCategories()).Where(c => c.Id == product.CategoryId).FirstOrDefault().Name ?? "Unknown",
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Kilograms = $"{product.Kilograms}",
                Price = $"{product.Price}",
                Id = product.Id,
            };
            ViewBag.Categories = (await categoriesService.GetAllCategories()).ToList();
            ViewBag.KilosList = new List<int>() { 1,2,3,4,5, 10, 15, 20, 25, 30, 35, 40, 45 };
            return await Task.Run(() => View(model));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(ProductsViewModel model)
        {
            await productsService.UpdateProduct(model);
            return await Task.Run(() => Redirect("/Products/ListProducts"));
        }


        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateOrder()
        { return await Task.Run(() => View()); }


        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateOrder(string? whatever)
        {
            string to = "*"; //To address    
            string from = "*"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Здравейте това е автоматизирано съобщение от Virtus-4 (VSMS). Тук ще бъдат получавани и изпращани бъдещи заявки.\n Приятен ден!";
            message.Subject = "Пробна връзка от VSMS";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("*", "*");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                return await Task.Run(() => Redirect("/Products/CreateOrder"));
            }
            return await Task.Run(() => Redirect("/Products/CreateOrder"));
        }


        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Quantities()
        {
            ViewBag.Products = await productsService.GetAllProducts();
            return await Task.Run(() => View());
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delivery()
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
                    Price = p.Price,
                    Quantity = p.Quantity
                });
            }
            ViewBag.Categories = categoriesList;
            return await Task.Run(() => View(model));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delivery(string deliveryJSON)
        {
            if (deliveryJSON == null)
            { return await Task.Run(() => Redirect("/Products/Delivery")); }
            if (deliveryJSON.Length <= 0)
            { return await Task.Run(() => Redirect("/Products/Delivery")); }

            productsService.RegisterDelivery(deliveryJSON);
            return await Task.Run(() => Redirect("/Products/Delivery"));
        }
    }
}
