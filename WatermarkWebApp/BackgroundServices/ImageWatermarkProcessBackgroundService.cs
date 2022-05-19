using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Drawing;
using System.Text;
using System.Text.Json;
using WatermarkWebApp.Services;

namespace WatermarkWebApp.BackgroundServices
{
    public class ImageWatermarkProcessBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitMQClientService;
        private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;
        private IModel _channel;

        public ImageWatermarkProcessBackgroundService(RabbitMQClientService rabbitMQClientService, ILogger<ImageWatermarkProcessBackgroundService> logger)
        {
            _rabbitMQClientService = rabbitMQClientService;
            _logger = logger;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClientService.Connect();

            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(
                RabbitMQClientService.QueueName,
                false,
                consumer);

            //consumer.Received += (sender, @event) => { }
            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                //resimn ismini alıyoruz
                var productImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", productImageCreatedEvent.ImageName);

                var imageText = "Berat Özmen";

                using var image = Image.FromFile(path);

                using var graphic = Graphics.FromImage(image);

                var font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Italic, GraphicsUnit.Pixel);

                var textSize = graphic.MeasureString(imageText, font);

                //Color.FromArgb(128, 255, 255, 255);
                var color = Color.AliceBlue;
                var brush = new SolidBrush(color);

                var position = new Point(image.Width - (int)textSize.Width + 5, image.Height - (int)textSize.Height + 5);

                graphic.DrawString(imageText, font, brush, position);

                image.Save("wwwroot\\images\\watermark\\" + productImageCreatedEvent.ImageName);

                image.Dispose();
                graphic.Dispose();

                // işlem başarılı bittiyse rabbitmq ya bildiriyoruz ve kuyruktan siliniyor.
                _channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
