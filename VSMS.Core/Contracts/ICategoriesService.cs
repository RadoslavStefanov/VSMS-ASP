using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Contracts
{
    public interface ICategoriesService
    {
        public Task<List<CategoryViewModel>> GetAllCategories();

        public Task Create(string name);

        public Task Delete(string name);
    }
}
