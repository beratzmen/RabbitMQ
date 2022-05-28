using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebExcelCreate.Models;
using WebExcelCreate.Services;

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

/// //Rabbitmq connection stringi configten çekilerek, DI olaran eklendi.
builder.Services.AddSingleton(sp =>
    new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")),
        DispatchConsumersAsync = true //Asenkron metot kullanýldýðýný bildirdik
    });

builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();
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
        userManager.CreateAsync(new IdentityUser { UserName = "deneme41", Email = "deneme41@gmail.com" }, "Deneme.41").Wait();
        userManager.CreateAsync(new IdentityUser { UserName = "berat2", Email = "berat2@gmail.com" }, "beraT.2").Wait();
    }
}
//////*****************************************************************************************************//////

app.Run();
