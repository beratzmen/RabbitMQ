
using ClosedXML.Excel;
using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Data;
using System.Text;
using System.Text.Json;

namespace FileCreateWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitMQClientService;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        public Worker(RabbitMQClientService rabbitMQClientService, IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _rabbitMQClientService = rabbitMQClientService;
            _serviceProvider = serviceProvider;
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

            _channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(5000);

            var createExcelMessage = JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            using var memoryStream = new MemoryStream();

            var workBook = new XLWorkbook();

            var dataSet = new DataSet();

            dataSet.Tables.Add(GetTable("Products"));

            workBook.Worksheets.Add(dataSet);

            workBook.SaveAs(memoryStream);

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "file", Guid.NewGuid().ToString() + ".xlsx");

            var baseUrl = "http://localhost:5160/api/files";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync($"{baseUrl}?fileId={createExcelMessage.FileId}", multipartFormDataContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"File (Id: {createExcelMessage.FileId}) was created by successful");

                    _channel.BasicAck(@event.DeliveryTag, false);
                }
            }
        }

        private DataTable GetTable(string tableName)
        {
            List<Models.Product> products;

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();

                products = context.Product.ToList();
            }

            DataTable table = new DataTable
            {
                TableName = tableName
            };

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));

            products.ForEach(x =>
            {
                table.Rows.Add(x.Id, x.Name);
            });

            return table;
        }
    }
}