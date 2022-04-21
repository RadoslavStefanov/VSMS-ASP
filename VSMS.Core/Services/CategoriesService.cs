using VSMS.Core.Contracts;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly Repository repo;
        public CategoriesService(Repository _repo)
        { repo = _repo; }


        public async Task<List<CategoryViewModel>> GetAllCategories()
        {
            var categories = repo.All<Categories>().ToList();
            var list = new List<CategoryViewModel>();
            foreach (var item in categories)
            {
                list.Add(new CategoryViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            list.Remove(list[0]);
            return list;
        }

        public async Task Create(string name)
        {
            Categories? catg = null;

            try { catg = new Categories { Name = name }; await repo.AddAsync(catg); }
            catch (Exception) { throw new ArgumentException("The category has not been created"); }
            
            await repo.SaveChangesAsync();
        }

        public async Task Delete(string name)
        {
            Categories? catg = null;

            try 
            {
                catg = repo.All<Categories>().Where(c => c.Name == name).FirstOrDefault();
                if (catg == null) { throw new Exception(); }
                await repo.DeleteAsync<Categories>(catg.Id);
                await repo.SaveChangesAsync();
            }
            catch (Exception) { throw new ArgumentException("The category has not been deleted"); }

           
        }
    }
}
