using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WatermarkWebApp.BackgroundServices;
using WatermarkWebApp.Models;
using WatermarkWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//////*****************************************************************************************************//////
/*Inmemory Db ConnectionString*/
builder.Services.AddDbContext<Context>(options =>
{
    options.UseInMemoryDatabase(databaseName: "productDb");
});

//Rabbitmq connection stringi configten çekilerek, DI olaran eklendi.
builder.Services.AddSingleton(sp =>
    new ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")),
        DispatchConsumersAsync = true //Asenkron metot kullanýldýðýný bildirdik
    });

builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<ImageWatermarkProcessBackgroundService>();
//////*****************************************************************************************************//////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
