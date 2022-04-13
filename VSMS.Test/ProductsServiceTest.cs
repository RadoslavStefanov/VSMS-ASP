using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Test
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
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<Repository>();
            await repo.SaveChangesAsync();
        }

        [Test]
        public void ShoudlCreateRequestIfUserExistsAndItsHisFirstTime()
        {
            var service = serviceProvider.GetService<HelpService>();
            Assert.DoesNotThrowAsync(async () => await service.CreateResetRequest("admin@virtus.bg"));
        }

        [TearDown]
        public void TearDown()
        { dbContext.Dispose(); }
    }
}
