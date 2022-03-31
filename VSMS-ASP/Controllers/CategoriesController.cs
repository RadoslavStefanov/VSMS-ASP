﻿using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;

namespace VSMS_ASP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoriesService categoriesService;
        public CategoriesController(CategoriesService _categoriesService)
        {
            categoriesService = _categoriesService;
        }

        public IActionResult ListCategories()
        {
            ViewData["View"] = "Categories";
            var categories = categoriesService.GetAllCategories();
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
            return View(list);
        }
        public void Create(string arg)
        { 
            categoriesService.Create(arg);
            Response.Redirect("/Categories/ListCategories");
        }

        public void Delete(string arg)
        {
            categoriesService.Delete(arg);
            Response.Redirect("/Categories/ListCategories");
        }
    }
}