using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
