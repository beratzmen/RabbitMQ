
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

var queueName = "direct-queue-Error";

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor..");

/// <summary>Rabbitmq subscriber'a mesaj gönderdiği zaman bu metot çalışır.</summary>
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine($"Rabbitmq gelen mesaj: {message}");

    Thread.Sleep(1000);

    File.AppendAllText("logs-Error.txt", message + "\n");

    /// <summary>Gelen mesaj başarılı işlendiyse bu metot ile ilgili mesajı sileriz.</summary>
    /// <param name="deliveryTag">Rabbitmq hangi TAG ile mesajı ilettiyse o mesajı siliyoruz.</param>
    /// <param name="multiple">True verilirse, o anda memory'de işlenmiş ama rabbitmq gitmemiş mesajlar varsa haber verilir. False verilirse, her bir bilgi ile ilgili mesajın durumunu rabbitmq'ya bildiririz.</param>
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();