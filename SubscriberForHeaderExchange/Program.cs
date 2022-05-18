
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

var dictionary = new Dictionary<string, object>();
dictionary.Add("format", "pdf");
dictionary.Add("shape", "a4");
//'All' - Tüm değerlerin eşleşmesi gerekir(Publisher). All yerine 'Any' yapılırsa, eşleşen herhangi biri alınır.
dictionary.Add("x-match", "all"); 

channel.QueueBind(queueName, "header-exchange", string.Empty, dictionary);

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor..");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    var product = JsonSerializer.Deserialize<Product>(message);

    Console.WriteLine($"Rabbitmq gelen mesaj: {product.Id} - {product.Name} - {product.Price} - {product.Stock}");

    channel.BasicAck(e.DeliveryTag, false);

    Thread.Sleep(1000);
};

Console.ReadLine();