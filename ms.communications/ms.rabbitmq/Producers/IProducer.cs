using ms.rabbitmq.Events;

namespace ms.rabbitmq.Producers
{
    public interface IProducer
    {
        void SendMessage(IRabbitMqEvent rabbitmqEvent);
    }
}
