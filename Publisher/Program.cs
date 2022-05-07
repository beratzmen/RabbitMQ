
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

/// <summary>Kuyruk oluşturma metodu. Parametreleri publisher-subscriber aynı olmalıdır. Eğer bu method eklenmezse, kuyruk yoksa hata verir.</summary>
/// <param name="queue">Kuyruk ismi.</param>
/// <param name="durable">False yapılırsa, rabbitmq'da oluşan kuyruklar memory'de tutulur. True yapılırsa fiziksel olarak kaydedilir restart edilse bile kuyruklar kaybolmaz</param>
/// <param name="exclusive">True yapılırsa sadece buradaki kanal üzerinden bağlanılabilir, farklı kanallar(Instance) üzerinden bağlanılmak istenirse FALSE yapılmalıdır.</param>
/// <param name="auto delete">True yapılırsa,kuyruğa bağlı olan son subscriber bağlantısını kopartırsa kuyruk silinir.</param>
channel.QueueDeclare("queue", true, false, false);

string message = "";

for (int i = 1; i <= 50; i++)
{
    message = $"Message: {i}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    /// <summary>Mesajı kuyruğa yayınlayan metot.</summary>
    /// <param name="exchange">Exchange kullanılmadığı, direk olarak kuyruğa gönderildiği için Empty geçilir.</param>
    /// <param name="routingKey">Exchange kullanılmazsa Default Exchange olarak kullanılır. Mutlaka kuyruğun ismi verilir.</param>
    /// <param name="body">Byte olarak mesaj içeriği verilir.</param>
    channel.BasicPublish(string.Empty, "queue", null, messageBody);

    Thread.Sleep(1500);

    Console.WriteLine($"Mesaj Gönderildi: {message}");
}

Console.ReadLine();