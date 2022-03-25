using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Contracts
{
    public interface IProductsService
    {
        public void Delete(string arg);
        public void Create(ProductsViewModel model);
        public List<Products> GetAllProducts();
        public string GetCategoryById(int id);
    }
}
