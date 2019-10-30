using RabbitMQ.Client;

namespace BlackJackAPI2.EventBus
{
    public interface IRabbitMQPersistentConnection
    {
        bool IsConnected { get; }

        void CreateConsumerChannel();
        IModel CreateModel();
        void Disconnect();
        void Dispose();
        bool TryConnect();
    }
}