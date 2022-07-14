using RabbitMQ.Client;
using RegistirationDirectory.SharedLibrary;
using RegistrationDirectory.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }
        public void PublishForCreatePicture(CreatePictureMessage createPictureMessage)
        {
            var channel = _rabbitMQClientService.ConnectForCreatePicture();
            var bodyString = JsonSerializer.Serialize(createPictureMessage);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: RabbitMQClientService.ExcangeName, routingKey: RabbitMQClientService.RoutingNameForPicture, basicProperties: properties, body: bodyByte);
        }
        public void PublishForWatermark(CreatePictureWithWatermarkMessage createPictureWithWatermarkMessage)
        {
            var channel = _rabbitMQClientService.ConnectForWatermark();
            var bodyString = JsonSerializer.Serialize(createPictureWithWatermarkMessage);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: RabbitMQClientService.ExcangeName, routingKey: RabbitMQClientService.RoutingNameForWatermark, basicProperties: properties, body: bodyByte);
        }
    }
}
