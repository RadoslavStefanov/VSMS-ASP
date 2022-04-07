using VSMS.Core.Contracts;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly Repository repo;
        public CategoriesService(Repository _repo)
        { repo = _repo; }


        public async Task<List<Categories>> GetAllCategories()
        {return repo.All<Categories>().ToList();}

        public async Task Create(string name)
        {
            var catg = new Categories{ Name = name };
            await repo.AddAsync(catg);
            await repo.SaveChangesAsync();
        }

        public async Task Delete(string name)
        {
            var catg = repo.All<Categories>().Where(c => c.Name == name).FirstOrDefault();
            await repo.DeleteAsync<Categories>(catg.Id);
            await repo.SaveChangesAsync();
        }
    }
}
