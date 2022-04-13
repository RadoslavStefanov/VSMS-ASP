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
        public ProductsService(Repository _repo)
        { repo = _repo; }

        private class QuantityAdder
        {
            public string? ProductName { get; set; }
            public string? AddedAmount { get; set; }
        }

        public async Task Create(ProductsViewModel model)
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
                Price = decimal.Parse(model.Price),
                Id = model.Id
            };
            await repo.AddAsync(newProduct);
            await repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var p = repo.All<Products>().Where(p => p.Id == id).FirstOrDefault();
                if (p == null) { return true; }

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

        public async Task<string> GetCategoryById(int id)
        { return repo.All<Categories>().Where(c => c.Id == id).FirstOrDefault().Name ?? "Category was not found in DB!"; }

        public async Task RegisterDelivery(string JSONinput)
        {
            var result = JsonSerializer.Deserialize<List<QuantityAdder>>(JSONinput);
            foreach (var item in result)
            {
                var product = repo.All<Products>().Where(p => p.Name == item.ProductName).FirstOrDefault();
                product.Quantity += decimal.Parse(item.AddedAmount);
            }
            await repo.SaveChangesAsync();
        }
    }
}
