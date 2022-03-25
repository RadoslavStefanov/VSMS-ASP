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


        public List<Categories> GetAllCategories()
        {return repo.All<Categories>().ToList();}

        public void Create(string name)
        {
            var catg = new Categories{ Name = name };
            repo.Add(catg);
            repo.SaveChanges();
        }

        public void Delete(string name)
        {
            var catg = repo.All<Categories>().Where(c => c.Name == name).FirstOrDefault();
            repo.Remove(catg);
            repo.SaveChanges();
        }
    }
}
