
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

//Publisher tarafında Exchange oluşturulduğu için aşağıdaki satıra gerek yok. Exchange oluşturulduysa aşağıdaki satırın aktif olması sorun yaratmaz. Fakat publisher ile subscriber tarafındaki ayarlar farklı olursa hata verir.
//channel.ExchangeDeclare("logs", durable: true, type: ExchangeType.Fanout);

//Birden fazla Client(Subscriber) exchange'e kuyruk oluşturucağı için hepsinin birbirinden farklı kuyruk isimleri olması için eklendi. Guid'de verilebilirdi.
var randomQueueName = channel.QueueDeclare().QueueName;

//Kuyruk ismi kalıcı olsun istenirse aşağıdaki şekilde yapılması gerekir. Client(Subscriber) down olsa bile kuyruklar verilerle beraber hazır bekler, tekrar kuyruğa bağlanıldığında kaldığı verden verileri tüketmeye devam eder.
//randomQueueName = "kalıcıkuyrukismi";
//channel.QueueDeclare(randomQueueName, true, false, false);

/// <summary>Kuyruk oluşturma metodu.</summary>
/// <param name="queue">Kuyruk ismi.</param>
/// <param name="exchange">Publisher tarafında oluşturulan Exchange ismini verdik.</param>
/// <param name="root">Boş olduğu için boş olarak geçildi.</param>
channel.QueueBind(randomQueueName, "logs", "", null);

/// <summary>Her bir subscriber'a kaç mesaj iletilecek.</summary>
/// <param name="prefetchCount">Her bir subscriber'a 1 mesaj gelsin.</param>
/// <param name="global">True yapılırsa, kaç subscriber varsa tek bir seferde subscriber'ların sayısına bölerek gönderir. False yapılırsa her subscriber için kaçar tane gönderileceği ayarlanır.</param>
channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

/// <summary>İlgili queue'da işlem başarılıysa bildirim işlemi yapılır ve kaydı siler.</summary>
/// <param name="queue">Kuyruk ismi.</param>
/// <param name="autoAck">True verilirse, kuyruk mesajı subscriber'a gönderdiği zaman kuyruktan mesajı siler. False verilirse, kuyruktan silinmez mesaj doğru işlenmişse kuyruğa haber verilir ve kuyruktan mesaj silinir.</param>
/// <param name="IBasicConsumer">Consumer.</param>
channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine("Loglar dinleniyor..");

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