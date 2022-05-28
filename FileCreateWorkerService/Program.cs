using FileCreateWorkerService;
using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        //Rabbitmq connection stringi configten çekilerek, DI olaran eklendi. Rabbitmq register edildi
        services.AddSingleton<RabbitMQClientService>();

        //Sql connection string ekledik
        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        //Rabbitmq connection string ekledik
        services.AddSingleton(sp =>
            new ConnectionFactory()
            {
                Uri = new Uri(configuration.GetConnectionString("RabbitMQ")),
                DispatchConsumersAsync = true //Asenkron metot kullanýldýðýný bildirdik
            });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
