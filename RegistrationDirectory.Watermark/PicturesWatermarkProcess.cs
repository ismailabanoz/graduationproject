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
    internal class PicturesWatermarkProcess:BackgroundService
    {
        private readonly ILogger<PicturesWatermarkProcess> _logger;
        private readonly RabbitMQClientService _rabbitmqClientService;
        private IModel _channel;
        private readonly ICustomerService _customerService;

        public PicturesWatermarkProcess(ILogger<PicturesWatermarkProcess> logger, RabbitMQClientService rabbitmqClientService, ICustomerService customerService)
        {
            _logger = logger;
            _rabbitmqClientService = rabbitmqClientService;
            _customerService = customerService;
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
                await Task.Delay(5000, stoppingToken);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var consumer = new AsyncEventingBasicConsumer(_channel);
                _channel.BasicConsume(RabbitMQClientService.QueueNameForWatermark, false, consumer);
                consumer.Received += AddWatermark;
                Task.CompletedTask.Wait();
            }
            
        }

        private Task AddWatermark(object sender, BasicDeliverEventArgs @event)
        {
            
            try
            {
                var picturesWatermarkProcess = JsonSerializer.Deserialize<CreatePictureWithWatermarkMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                var siteName = "www.mysite.com";
                var photos = Path.GetFullPath(@"..\RegistrationDirectory.API\photos");
                 var path=   Path.Combine(photos, picturesWatermarkProcess.ImageName);
                using var img = Image.FromFile(path);
                using var graphic = Graphics.FromImage(img);
                var font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold, GraphicsUnit.Pixel);

                var textSize = graphic.MeasureString(siteName, font);

                var color = Color.FromArgb(255, 255, 0, 0);
                var brush = new SolidBrush(color);

                var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));


                graphic.DrawString(siteName, font, brush, position);
                img.Save(@"..\RegistrationDirectory.API\PhotosWithWatermark\" + picturesWatermarkProcess.ImageName + "(with watermark).jpg");
                
                
                byte[] imageBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    img.Save(memoryStream, img.RawFormat);
                    imageBytes = memoryStream.ToArray();
                }
                /*_customerService.Update(new Customer
                {
                    Id = picturesWatermarkProcess.CustomerId,
                    Photograph = imageBytes
                });*/
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
