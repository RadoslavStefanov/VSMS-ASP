using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Server=.;Database=VSMS;Trusted_Connection=True;Integrated Security=True;");
builder.Services.AddDbContext<VSMSDbContext>(options =>
    options.UseSqlServer("Server=.;Database=VSMS;Trusted_Connection=True;Integrated Security=True;"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<VSMSDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<UsersService>();
builder.Services.AddTransient<Repository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ app.UseMigrationsEndPoint();}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Error/Error404";
        await next();
    }
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
