using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VSMS.Infrastructure.Data;

namespace VSMS.Test
{
    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<VSMSDbContext> dbContextOptions;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<VSMSDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new VSMSDbContext(dbContextOptions);

            context.Database.EnsureCreated();
        }

        public VSMSDbContext CreateContext() => new VSMSDbContext(dbContextOptions);

        public void Dispose() => connection.Dispose();
    }
}
