using VSMS.Core.ViewModels;

namespace VSMS.Core.Contracts
{
    public interface ICategoriesService
    {
        public Task<List<CategoryViewModel>> GetAllCategories();

        public Task Create(string name);

        public Task Delete(string name);
    }
}
