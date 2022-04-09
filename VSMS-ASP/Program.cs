using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VSMS.Core.Services;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data;
using VSMS_ASP.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = "Server =.; Database = VSMS; Trusted_Connection = True; Integrated Security = True;";
var connectionString = builder.Configuration.GetConnectionString(connection);
builder.Services.AddDbContext<VSMS_ASPContext>(options =>
    options.UseSqlServer(connection));builder.Services.AddDbContext<VSMSDbContext>(options =>
    options.UseSqlServer(connection));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<VSMSDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<Repository>();
builder.Services.AddTransient<ProductsService>();
builder.Services.AddTransient<CategoriesService>();
builder.Services.AddTransient<SalesService>();
builder.Services.AddTransient<HelpService>();
builder.Services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

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
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
