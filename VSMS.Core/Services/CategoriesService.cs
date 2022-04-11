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
