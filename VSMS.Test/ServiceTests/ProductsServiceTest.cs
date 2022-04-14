using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Test.ServiceTests
{
    public class ProductsServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<Repository, Repository>()
                .AddSingleton<ProductsService, ProductsService>()
                .AddSingleton<CategoriesService, CategoriesService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<Repository>();
            await repo.SaveChangesAsync();
        }

        [Test]
        public void ShouldThrowOnCreateIfModelIsInvalid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            Assert.ThrowsAsync<ArgumentException>(async () => 
            await service.Create(new Core.ViewModels.ProductsViewModel 
            {Name=null, Category=null, ImageUrl=null, Description=null, Kilograms=null, Price=null }));
        }

        [Test]
        public async Task ShouldNotThrowOnCreateIfModelIsValid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();

            await categoriesService.Create("Test");
            Assert.DoesNotThrowAsync(async () =>
            await service.Create(new Core.ViewModels.ProductsViewModel
            { 
                Name = "Пробен", 
                Category = repo.All<Categories>().FirstOrDefault().Name, 
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png", 
                Description = "Just a simple tester object",
                Kilograms = "10", 
                Price = "12.50" 
            }));
        }

        [Test]
        public async Task ShouldThrowOnDeleteIfIdIsInvalid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            Assert.False(await service.DeleteById(-1));
        }

        [Test]
        public async Task ShouldGetAllProducts()
        {
            var service = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();

            await categoriesService.Create("Test");

            await service.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });

            var result = await service.GetAllProducts();

            Assert.True(result.Count>=1);
        }

        [Test]
        public async Task ShouldDeleteIfValid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();

            await categoriesService.Create("Test");

            await service.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });

            var productID = (await service.GetAllProducts()).FirstOrDefault().Id;
            Assert.DoesNotThrowAsync(async () => await service.DeleteById(productID));
        }

        [Test]
        public async Task ShouldReturnNotFoundIfIdIsInvalid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            var result = await service.GetCategoryById(-234);
            Assert.AreEqual("Category was not found in DB!",result);
        }

        [Test]
        public async Task ShouldThrowOnRegisterIfJsonIsNotValid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            Assert.ThrowsAsync<ArgumentException>(async () => await service.RegisterDelivery(""));
        }

        [Test]
        public async Task ShouldThrowOnRegisterIfProductIsNull()
        {
            var service = serviceProvider.GetService<ProductsService>();
            Assert.ThrowsAsync<ArgumentException>(async () => await service.RegisterDelivery("[{\"ProductName\":\"Бройлери Стартер 10кг.\",\"AddedAmount\":\"2\"}]"));
        }

        [Test]
        public async Task ShouldRegisterIfAllIsValid()
        {
            var service = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();

            await categoriesService.Create("Test");

            await service.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });

            Assert.DoesNotThrowAsync(async () => await service.RegisterDelivery("[{\"ProductName\":\"Пробен\",\"AddedAmount\":\"2\"}]"));
        }

        [TearDown]
        public void TearDown()
        { dbContext.Dispose(); }
    }
}
