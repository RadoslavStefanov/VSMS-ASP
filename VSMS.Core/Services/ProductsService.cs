﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSMS.Core.Contracts;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly Repository repo;
        public ProductsService(Repository _repo)
        { repo = _repo; }

        public void Create(ProductsViewModel model)
        {
            int categoryId=0;
            if (repo.All<Categories>().Where(c => c.Name == model.Category) != null )
            { categoryId = repo.All<Categories>().Where(c => c.Name == "Unknown").FirstOrDefault().Id; }

            var newProduct = new Products
            {
                Name = model.Name,
                CategoryId = categoryId,
                Kilograms = int.Parse(model.Kilograms),
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = decimal.Parse(model.Price)
            };
            repo.Add(newProduct);
            repo.SaveChanges();
        }

        public void Delete(string arg)
        {
            throw new NotImplementedException();
        }

        public List<Products> GetAllProducts()
        {return repo.All<Products>().ToList();}

        public string GetCategoryById(int id)
        { return repo.All<Categories>().Where(c => c.Id == id).FirstOrDefault().Name ?? "Category was not found in DB!"; }
    }
}
