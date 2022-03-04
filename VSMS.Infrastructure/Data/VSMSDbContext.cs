namespace VSMS.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using VSMS.Infrastructure.Data.Models;

    public class VSMSDbContext : IdentityDbContext
    {

        public VSMSDbContext(DbContextOptions<VSMSDbContext> options)
                : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=VSMS;Trusted_Connection=True;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesProducts>()
                .HasKey(t => new { t.SaleId, t.ProductId });
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SalesProducts> SalesProducts { get; set; }
    }
}
