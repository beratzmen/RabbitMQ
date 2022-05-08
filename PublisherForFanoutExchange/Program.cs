
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

/// <summary>Mesajlar direk kuyruk yerine, Exchange gönderilecek. Bağlanacak olan client'lar(Subscriberlar) kendi kuyruklarını oluşturacak ve verileri tüketecek.</summary>
/// <param name="exchange">Exchange ismi.</param>
/// <param name="durable">Uygulama restart edilsede kaybolmaz. False yapılırsa exchangeler kaybolur.</param>
/// <param name="type">Exchange tipi fanout olacak.</param>
channel.ExchangeDeclare("logs", durable:true, type: ExchangeType.Fanout);

string message = "";

for (int i = 1; i <= 50; i++)
{
    Thread.Sleep(1500);

    message = $"Message: {i}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    /// <summary>Mesajı kuyruğa yayınlayan metot.</summary>
    /// <param name="exchange">Fanout exchange tipini kullandığımız için, exchange ismini verdik.</param>
    /// <param name="routingKey">Exchange kullanıldığı için Kuyruk ismini empty geçtik. Kullanılmazsa Default Exchange olarak kullanılır. Mutlaka kuyruğun ismi verilir.</param>
    /// <param name="body">Byte olarak mesaj içeriği verilir.</param>
    channel.BasicPublish("logs", String.Empty, null, messageBody);

    Console.WriteLine($"Mesaj Gönderildi: {message}");
}

Console.ReadLine();