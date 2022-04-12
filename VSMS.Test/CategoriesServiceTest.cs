using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Test
{
    public class CategoriesServiceTest
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
                .AddSingleton<CategoriesService, CategoriesService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<Repository>();
            var category = new Categories() { Name = "Прасета" };

            await repo.AddAsync(category);
            await repo.SaveChangesAsync();
        }

        [Test]
        public void UnknownCategoryShouldThrowOnDelete()
        {
            var service = serviceProvider.GetService<CategoriesService>();
            Assert.CatchAsync<Exception>(async () => await service.Delete("nonExistingCategory"));
        }

        [Test]
        public void KnownCategoryShouldNotThrowOnDelete()
        {
            var service = serviceProvider.GetService<CategoriesService>();
            Assert.DoesNotThrowAsync(async () => await service.Delete("Прасета"));
        }

        /*[Test]
        public async Task KnownCategoryShouldBeDeleted()
        {
            var service = serviceProvider.GetService<CategoriesService>();
            await service.Delete("Прасета");
            Assert.AreEqual()
        }*/

        public void TearDown()
        {dbContext.Dispose();}
    }
}