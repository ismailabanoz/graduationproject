using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RegistirationDirectory.SharedLibrary;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using RegistrationDirectory.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RegistrationDirectory.Watermark
{
    
    public class PicturesWatermarkProcess:BackgroundService
    {
        private readonly ILogger<PicturesWatermarkProcess> _logger;
        private readonly RabbitMQClientService _rabbitmqClientService;
        private IModel _channel;
        private IServiceProvider _serviceProvider;
        public PicturesWatermarkProcess(ILogger<PicturesWatermarkProcess> logger, RabbitMQClientService rabbitmqClientService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitmqClientService = rabbitmqClientService;
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel=_rabbitmqClientService.ConnectForWatermark();
            _channel.BasicQos(0, 1, false);
            return base.StartAsync(cancellationToken);
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var consumer = new AsyncEventingBasicConsumer(_channel);
                _channel.BasicConsume(RabbitMQClientService.QueueNameForWatermark, false, consumer);
                consumer.Received += AddWatermark;
                Task.CompletedTask.Wait();
                await Task.Delay(1000*60*60, stoppingToken);
            }
            
        }

        private async Task<Task> AddWatermark(object sender, BasicDeliverEventArgs @event)
        {
            
            try
            {
                var customer = JsonSerializer.Deserialize<Customer>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                var siteName = "www.mysite.com";
                var photos = Path.GetFullPath(@"..\RegistrationDirectory.API\photos");
                var imageName = customer.Id.ToString() + customer.Name + customer.Surname + ".jpg";
                var path=   Path.Combine(photos, imageName);
                using var img = Image.FromFile(path);
                using var graphic = Graphics.FromImage(img);
                var font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold, GraphicsUnit.Pixel);
                var textSize = graphic.MeasureString(siteName, font);
                var color = Color.FromArgb(255, 255, 0, 0);
                var brush = new SolidBrush(color);
                var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));
                graphic.DrawString(siteName, font, brush, position);
                
                
                byte[] imageBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    img.Save(memoryStream, img.RawFormat);
                    imageBytes = memoryStream.ToArray();
                }


                using (var scope= _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    context.Customers.Update(new Customer
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Surname= customer.Surname,
                        Email= customer.Email,
                        Phone= customer.Phone,
                        City= customer.City,
                        Photograph = imageBytes
                    });
                    context.SaveChanges();
                }
                img.Save(@"..\RegistrationDirectory.API\PhotosWithWatermark\" + imageName + "(with watermark).jpg");
                img.Dispose();
                graphic.Dispose();
                _channel.BasicAck(@event.DeliveryTag, false);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
