using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Test
{
    public class SalesServiceTest
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
                .AddSingleton<SalesService, SalesService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<Repository>();
        }

        [Test]
        public void RegisterSaleShouldThrowIfJSONIsInvalid()
        {
            var service = serviceProvider.GetService<SalesService>();
            Assert.ThrowsAsync<ArgumentException>(async () => await service.RegisterSale("",""));
        }


        [TearDown]
        public void TearDown()
        { dbContext.Dispose(); }
    }
}
