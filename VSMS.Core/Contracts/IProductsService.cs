using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Contracts
{
    public interface IProductsService
    {
        public Task Create(ProductsViewModel model);

        public Task<bool> DeleteById(int id);

        public Task<List<Products>> GetAllProducts();

        public Task<string> GetCategoryById(int id);

        public Task RegisterDelivery(string JSONinput);

        public Task UpdateProduct(ProductsViewModel model);
    }
}
