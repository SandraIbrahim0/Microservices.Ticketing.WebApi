namespace Ticketing.Microservice.Core.Application.Contracts
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
