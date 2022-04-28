using System.Globalization;
using System.Text.Json;
using VSMS.Core.Contracts;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly Repository repo;
        private readonly CategoriesService categoriesService;
        public ProductsService(Repository _repo, CategoriesService _categoriesService)
        { repo = _repo; categoriesService = _categoriesService; }

        private class QuantityAdder
        {
            public string? ProductName { get; set; }
            public string? AddedAmount { get; set; }
        }

        public async Task Create(ProductsViewModel model)
        {
            if (!isModelValid(model)) { throw new ArgumentException("The provided model is not valid!"); }

            try
            {
                int categoryId = 0;
                if (repo.All<Categories>().Where(c => c.Name == model.Category) != null)
                { categoryId = repo.All<Categories>().Where(c => c.Name == model.Category).FirstOrDefault().Id; }

                var newProduct = new Products
                {
                    Name = model.Name,
                    CategoryId = categoryId,
                    Kilograms = int.Parse(model.Kilograms),
                    Description = model.Description ?? " ",
                    ImageUrl = model.ImageUrl,
                    Price = decimal.Parse(model.Price, CultureInfo.InvariantCulture),
                    Id = model.Id
                };
                await repo.AddAsync(newProduct);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            { throw new ArgumentException("The provided model is not valid!"); }

        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var p = repo.All<Products>().Where(p => p.Id == id).FirstOrDefault();
                if (p == null) { return false; }

                await repo.DeleteAsync<Products>(p.Id);
                await repo.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            { return false; }
        }

        public async Task UpdateProduct(ProductsViewModel model)
        {
            var product = repo.All<Products>().Where(p => p.Id == model.Id).FirstOrDefault();
            if (model.Description == null) { product.Description = ""; }
            else { product.Description = model.Description; }
            product.ImageUrl = model.ImageUrl;
            product.Price = decimal.Parse(model.Price, CultureInfo.InvariantCulture);
            product.Name = model.Name;
            product.CategoryId = repo.All<Categories>().Where(c => c.Name == model.Category).FirstOrDefault().Id;
            product.Kilograms = int.Parse(model.Kilograms);
            await Task.Run(() => repo.SaveChanges());

        }

        public async Task<List<Products>> GetAllProducts()
        { return repo.All<Products>().ToList(); }

        public async Task<List<AllProductsListViewModel>> GetAllAsModelList()
        {
            var categoriesList = await categoriesService.GetAllCategories();
            var productsList = await GetAllProducts();
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
            return model;
        }

        public async Task<ProductsViewModel> GetProductByIdAsModel(int id)
        {
            var product = repo.All<Products>().Where(p => p.Id == id).FirstOrDefault();
            if (product == null) { return null; }

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

            return model;
        }


        public async Task<string> GetCategoryById(int id)
        {
            var result = repo.All<Categories>().Where(c => c.Id == id).FirstOrDefault();
            if (result == null) { return "Category was not found in DB!"; }
            else { return result.Name; }
        }

        public async Task RegisterDelivery(string JSONinput)
        {
            var result = new List<QuantityAdder>();
            try
            { result = JsonSerializer.Deserialize<List<QuantityAdder>>(JSONinput); }
            catch (Exception)
            { throw new ArgumentException("Json input was invalid!"); }


            foreach (var item in result)
            {
                var product = repo.All<Products>().Where(p => p.Name == item.ProductName).FirstOrDefault();

                if (product == null) { throw new ArgumentException($"Product {item.ProductName} does not exist in the Database!"); }

                product.Quantity += decimal.Parse(item.AddedAmount, CultureInfo.InvariantCulture);
            }
            await repo.SaveChangesAsync();

        }

        private bool isModelValid(ProductsViewModel model)
        {
            if (model.Name == null
            || model.Name.Length > 35
            || model.Name.Length < 5)
            { return false; }

            if (model.Category == null)
            { return false; }

            if (model.Kilograms == null
            || int.Parse(model.Kilograms) < 0)
            { return false; }

            if (model.Description == null)
            { return false; }

            if (model.ImageUrl == null
            || model.ImageUrl.Length > 250)
            { return false; }

            if (model.Price == null
            || decimal.Parse(model.Price, CultureInfo.InvariantCulture) < 0)
            { return false; }

            return true;
        }
    }
}
