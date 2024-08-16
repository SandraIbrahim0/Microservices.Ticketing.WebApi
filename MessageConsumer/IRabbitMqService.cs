using RabbitMQ.Client;

namespace MessageConsumer
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }
}
