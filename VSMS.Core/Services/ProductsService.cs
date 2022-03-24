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
    public class ProductsService : IProductsService
    {
        private readonly Repository repo;
        public ProductsService(Repository _repo)
        { repo = _repo; }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Delete(string arg)
        {
            throw new NotImplementedException();
        }

        public List<Products> GetAllProducts()
        {return repo.All<Products>().ToList();}
    }
}
