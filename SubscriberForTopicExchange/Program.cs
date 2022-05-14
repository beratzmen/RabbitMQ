
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

//"*.*.Warning" => Sonunda Warning olan kuyruğa bağlanır. * tek bir karaktere karşılık gelir.
//Information.# => Başı Information olsun, sonunda ne var ne yok benim için önemli değil demek için # yapılır.
//Ortası Error olan başı ve sonu önemli olmayan route.
var routeKey = "*.Error.*";

//logs-topic exchange' indeki, başı sonu önemli değil ortasında Error yazan kuyruğa bağlan demek için.
channel.QueueBind(queueName, "logs-topic", routeKey);

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor..");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine($"Rabbitmq gelen mesaj: {message}");

    channel.BasicAck(e.DeliveryTag, false);

    Thread.Sleep(1000);
};

Console.ReadLine();