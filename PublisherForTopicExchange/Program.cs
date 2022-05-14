
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

string message = "";

for (int i = 1; i <= 50; i++)
{
    var random = new Random();
    var log1 = (LogNames)random.Next(1, 5);
    var log2 = (LogNames)random.Next(1, 5);
    var log3 = (LogNames)random.Next(1, 5);

    //Critical.Error.Information şeklinde bir kuyruk ismi oluşturmak için.
    var routeKey = $"{log1}.{log2}.{log3}";

    message = $"Log-Type: {log1}-{log2}-{log3}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-topic", routeKey, null, messageBody);

    Console.WriteLine($"Log Gönderildi: {message}");

    Thread.Sleep(1500);
}

Console.ReadLine();

enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Information = 4
}