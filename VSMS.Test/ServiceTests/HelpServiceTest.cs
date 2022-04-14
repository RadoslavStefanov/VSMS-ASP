using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;

namespace VSMS.Test.ServiceTests
{
    public class HelpServiceTest
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
                .AddSingleton<HelpService, HelpService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<Repository>();
        }

        [Test]
        public void ShoudlCreateRequestIfUserExistsAndItsHisFirstTime()
        {
            var service = serviceProvider.GetService<HelpService>();
            Assert.DoesNotThrowAsync(async () => await service.CreateResetRequest("admin@virtus.bg"));
        }

        [Test]
        public async Task ShoudlReturnFalseIfAlreadyCreated()
        {
            var service = serviceProvider.GetService<HelpService>();
            await service.CreateResetRequest("admin@virtus.bg");
            var result = await service.CreateResetRequest("admin@virtus.bg");
            Assert.False(result);
        }

        [Test]
        public async Task ShouldThrowIfNameIsNull()
        {
            var service = serviceProvider.GetService<HelpService>();
            Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateResetRequest(null));
        }

        [Test]
        public async Task ShouldThrowIfNameIsLessThan3Chars()
        {
            var service = serviceProvider.GetService<HelpService>();
            Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateResetRequest("aa"));
        }

        [Test]
        public async Task ShouldReturnAllRequests()
        {
            var service = serviceProvider.GetService<HelpService>();
            await service.CreateResetRequest("admin@virtus.bg");
            var result = service.GetAllRequests();
            Assert.True(result.Count>=1);
        }

        [TearDown]
        public void TearDown()
        { dbContext.Dispose(); }
    }
}
