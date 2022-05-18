
using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

var dictionary = new Dictionary<string, object>();

dictionary.Add("format", "pdf");
dictionary.Add("shape", "a4");

var properties = channel.CreateBasicProperties();

properties.Headers = dictionary;
//Mesajlar kalıcı hale getirildi. Rabbitmq restart edilse bile mesajlar kaybolmaz.
properties.Persistent = true;

var product = new Product
{
    Id = 1,
    Name = "Product1",
    Price = 5,
    Stock = 55
};

var productJsonString = JsonSerializer.Serialize(product);

channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));

Console.WriteLine("Mesaj gönderilmiştir.");

Console.ReadLine();

enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Information = 4
}