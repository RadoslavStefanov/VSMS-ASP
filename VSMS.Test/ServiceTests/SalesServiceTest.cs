using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Test.ServiceTests
{
    public class SalesServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<Repository, Repository>()
                .AddSingleton<SalesService, SalesService>()
                .AddSingleton<CategoriesService, CategoriesService>()
                .AddSingleton<ProductsService, ProductsService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<Repository>();
        }

        [Test]
        public void RegisterSaleShouldThrowIfJSONIsInvalid()
        {
            var service = serviceProvider.GetService<SalesService>();
            Assert.ThrowsAsync<ArgumentException>(async () => await service.RegisterSale("",""));
        }

        [Test]
        public void RegisterSaleShouldThrowIfAnyProductNameIsInvalid()
        {
            var service = serviceProvider.GetService<SalesService>();
            Assert.ThrowsAsync<ArgumentException>(async () => 
            await service.RegisterSale("[{\"soldProductName\":\"Unknown\",\"soldProductAmout\":1,\"soldProductTotalPrice\":10.5,\"AtPrice\":10.5}]", ""));
        }

        [Test]
        public async Task RegisterSaleShouldThrowIfAnyProductAmountIsInvalid()
        {
            var service = serviceProvider.GetService<SalesService>();
            var pService = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();

            await categoriesService.Create("Test");

            await pService.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });
            Assert.ThrowsAsync<ArgumentException>(async () =>
            await service.RegisterSale("[{\"soldProductName\":\"Пробен\",\"soldProductAmout\":0,\"soldProductTotalPrice\":10.5,\"AtPrice\":10.5}]", ""));
        }

        [Test]
        public async Task RegisterSaleShouldThrowIfAnyProductAtPriceIsInvalid()
        {
            var service = serviceProvider.GetService<SalesService>();
            var pService = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();

            await categoriesService.Create("Test");

            await pService.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });
            Assert.ThrowsAsync<ArgumentException>(async () =>
            await service.RegisterSale("[{\"soldProductName\":\"Пробен\",\"soldProductAmout\":1,\"soldProductTotalPrice\":10.5,\"AtPrice\":-1}]", ""));
        }

        [Test]
        public async Task RegisterSaleShouldWorkIfAllIsInvalid()
        {
            var service = serviceProvider.GetService<SalesService>();
            var pService = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            await categoriesService.Create("Test");

            await pService.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });

            var user = new IdentityUser() { Email="test@virtus.bg",Id= "fe4153fa-e9eb-4d8c-97b9-23c22b2f1f93" };
            await repo.AddAsync(user); await repo.SaveChangesAsync();

            Assert.DoesNotThrowAsync(async () =>
            await service.RegisterSale("[{\"soldProductName\":\"Пробен\",\"soldProductAmout\":1,\"soldProductTotalPrice\":10.5,\"AtPrice\":10}]", user.Id));
        }

        [Test]
        public void GetUserSalesShouldThrowIfUserIsNotValid()
        {
            var service = serviceProvider.GetService<SalesService>();

            Assert.ThrowsAsync<ArgumentException>(async () =>
            await service.GetUserSales("", ""));
        }

        [Test]
        public async Task GetUserSalesShouldNotThrowIfAllIsValid()
        {
            var service = serviceProvider.GetService<SalesService>();
            var pService = serviceProvider.GetService<ProductsService>();
            var categoriesService = serviceProvider.GetService<CategoriesService>();
            var repo = serviceProvider.GetService<Repository>();
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            await categoriesService.Create("Test");

            await pService.Create(new Core.ViewModels.ProductsViewModel
            {
                Name = "Пробен",
                Category = repo.All<Categories>().FirstOrDefault().Name,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
                Description = "Just a simple tester object",
                Kilograms = "10",
                Price = "12.50"
            });

            var user = new IdentityUser() { Email = "test@virtus.bg", Id = "fe4153fa-e9eb-4d8c-97b9-23c22b2f1f93" };
            await repo.AddAsync(user); await repo.SaveChangesAsync();

            await service.RegisterSale("[{\"soldProductName\":\"Пробен\",\"soldProductAmout\":1,\"soldProductTotalPrice\":10.5,\"AtPrice\":10}]", user.Id);

            Assert.DoesNotThrowAsync(async () =>
            await service.GetUserSales(user.Id, "test@virtus.bg"));
        }

        [Test]
        public async Task GetSAllSalesShouldNotThrow()
        {
            var service = serviceProvider.GetService<SalesService>();
            
            Assert.DoesNotThrowAsync(async () =>
            await service.GetSales());
        }

        [TearDown]
        public void TearDown()
        { dbContext.Dispose(); }
    }
}
