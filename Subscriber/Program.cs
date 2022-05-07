
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.QueueDeclare("queue", true, false, false);

/// <summary>Her bir subscriber'a kaç mesaj iletilecek.</summary>
/// <param name="prefetchCount">Her bir subscriber'a 1 mesaj gelsin.</param>
/// <param name="global">True yapılırsa, kaç subscriber varsa tek bir seferde subscriber'ların sayısına bölerek gönderir. False yapılırsa her subscriber için kaçar tane gönderileceği ayarlanır.</param>
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

/// <summary>İlgili queue'da işlem başarılıysa bildirim işlemi yapılır ve kaydı siler.</summary>
/// <param name="queue">Kuyruk ismi.</param>
/// <param name="autoAck">True verilirse, kuyruk mesajı subscriber'a gönderdiği zaman kuyruktan mesajı siler. False verilirse, kuyruktan silinmez mesaj doğru işlenmişse kuyruğa haber verilir ve kuyruktan mesaj silinir.</param>
/// <param name="IBasicConsumer">Consumer.</param>
channel.BasicConsume("queue", false, consumer);

/// <summary>Rabbitmq subscriber'a mesaj gönderdiği zaman bu metot çalışır.</summary>
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine($"Rabbitmq gelen mesaj: {message}");

    /// <summary>Gelen mesaj başarılı işlendiyse bu metot ile ilgili mesajı sileriz.</summary>
    /// <param name="deliveryTag">Rabbitmq hangi TAG ile mesajı ilettiyse o mesajı siliyoruz.</param>
    /// <param name="multiple">True verilirse, o anda memory'de işlenmiş ama rabbitmq gitmemiş mesajlar varsa haber verilir. False verilirse, her bir bilgi ile ilgili mesajın durumunu rabbitmq'ya bildiririz.</param>
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();