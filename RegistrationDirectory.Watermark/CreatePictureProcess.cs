using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RegistirationDirectory.SharedLibrary;
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
    internal class CreatePictureProcess :BackgroundService
    {
        private readonly ILogger<PicturesWatermarkProcess> _logger;
        private readonly RabbitMQClientService _rabbitmqClientService;
        private IModel _channel;
        public CreatePictureProcess(ILogger<PicturesWatermarkProcess> logger, RabbitMQClientService rabbitmqClientService)
        {
            _logger = logger;
            _rabbitmqClientService = rabbitmqClientService;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitmqClientService.ConnectForCreatePicture();
            _channel.BasicQos(0, 1, false);
            return base.StartAsync(cancellationToken);
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                var consumer = new AsyncEventingBasicConsumer(_channel);
                _channel.BasicConsume(RabbitMQClientService.QueueNameForPicture, false, consumer);
                consumer.Received += CreatePicture;
                Task.CompletedTask.Wait();
            }
            
        }

        private Task CreatePicture(object sender, BasicDeliverEventArgs @event)
        {

            try
            {
                var picturesWatermarkProcess = JsonSerializer.Deserialize<CreatePictureMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));
                byte[] imageBytes = picturesWatermarkProcess.BytePhoto;
                using var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                Image image = Image.FromStream(ms, true);
                var imageName = picturesWatermarkProcess.CustomerId.ToString() + picturesWatermarkProcess.Name + picturesWatermarkProcess.SurName + ".jpg";
                Bitmap bmp = new Bitmap(image.Width, image.Height);
                Graphics gra = Graphics.FromImage(bmp);
                gra.DrawImageUnscaled(image, new Point(0, 0));
                gra.Dispose();
                bmp.Save(@"..\RegistrationDirectory.API\Photos\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                bmp.Dispose();
                _channel.BasicAck(@event.DeliveryTag, false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
