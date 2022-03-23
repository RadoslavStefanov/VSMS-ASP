using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VSMS_ASP.Data;

public class VSMS_ASPContext : IdentityDbContext<IdentityUser>
{
    public VSMS_ASPContext(DbContextOptions<VSMS_ASPContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        Seed();
    }

    protected void Seed()
    {
        var passwordHash = new PasswordHasher();
        string password = passwordHash.HashPassword("123123");
        this.Users.Add
            (new IdentityUser
            {
                UserName = "admin@virtus.bg",
                PasswordHash = password,
                Email = "admin@virtus.bg"
            });
        this.Roles.Add(new IdentityRole { Id = "1", Name = "Admin" });
    }
}
