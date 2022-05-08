
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

foreach (var error in Enum.GetNames(typeof(LogNames)))
{
    var routeKey = $"route-{error}";

    var queueName = $"direct-queue-{error}";

    channel.QueueDeclare(queueName, true, false, false);

    channel.QueueBind(queueName, "logs-direct", routeKey, null);
}

string message = "";

for (int i = 1; i <= 50; i++)
{
    Thread.Sleep(1500);

    LogNames log = (LogNames)new Random().Next(1, 5);

    message = $"Log-Type: {log}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";

    channel.BasicPublish("logs-direct", routeKey, null, messageBody);

    Console.WriteLine($"Log Gönderildi: {message}");
}

Console.ReadLine();

enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Information = 4
}