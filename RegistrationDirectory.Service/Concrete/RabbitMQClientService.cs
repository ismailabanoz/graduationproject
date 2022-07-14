using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class RabbitMQClientService
    {
        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;
        private IModel _channel;
        private IModel _channel2;
        public static string ExcangeName = "excengeDirect";
        public static string RoutingNameForPicture = "pictureRoot";
        public static string RoutingNameForWatermark = "watermarkRoot";
        public static string QueueNameForPicture = "createPicture";
        public static string QueueNameForWatermark = "watermark";
        private readonly ILogger<RabbitMQClientService> _logger;
        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }
        public IModel ConnectForCreatePicture()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel is { IsOpen:true})
            {
                return _channel;
            }
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExcangeName, ExchangeType.Direct, true, false);
            _channel.QueueDeclare(QueueNameForPicture, true, false, false, null);
            _channel.QueueBind(exchange: ExcangeName, queue: QueueNameForPicture, routingKey: RoutingNameForPicture);
            _logger.LogInformation("Connected RabbitMQ");
            return _channel;
        }
        public IModel ConnectForWatermark()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel2 is { IsOpen: true })
            {
                return _channel2;
            }
            _channel2 = _connection.CreateModel();
            _channel2.ExchangeDeclare(ExcangeName, ExchangeType.Direct, true, false);
            _channel2.QueueDeclare(QueueNameForWatermark, true, false, false, null);
            _channel2.QueueBind(exchange: ExcangeName, queue: QueueNameForWatermark, routingKey: RoutingNameForWatermark);
            _logger.LogInformation("Connected RabbitMQ");
            return _channel2;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
            _logger.LogInformation("Disconnected RabbitMQ");
        }
    }
}
