using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using Ticketing.Microservice.Core.Application.Contracts;

namespace Ticketing.Microservice.Infrastructure.External
{
    public class NotificationService : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };
            factory.ClientProvidedName = "Rabbit Test";

            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            string exchangeName = "EmailExchange";
            string routingKey = "email_queue";
            string queueName = "EmailQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            channel.BasicQos(0, 1, false);

            string emailModelJson = JsonConvert.SerializeObject(message);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(emailModelJson);
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
        }
    }
}
