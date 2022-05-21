using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebExcelCreate.Models;

var builder = WebApplication.CreateBuilder(args);

//////*****************************************************************************************************//////
/*EFCORE Db ConnectionString*/
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<Context>();
//////*****************************************************************************************************//////

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//////*****************************************************************************************************//////
app.UseRouting();
app.UseAuthentication();
//////*****************************************************************************************************//////

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//////*****************************************************************************************************//////
///Program ayaða kalkarken seed kayýt eklendi
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    context.Database.Migrate();

    if (!context.Users.Any())
    {
        userManager.CreateAsync(new IdentityUser { UserName = "berat", Email = "berat@gmail.com" }, "berat").Wait();
        userManager.CreateAsync(new IdentityUser { UserName = "berat2", Email = "berat2@gmail.com" }, "berat2").Wait();
    }
}
//////*****************************************************************************************************//////

app.Run();
