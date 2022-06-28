using Microsoft.Extensions.Hosting;
using ms.rabbitmq.Consumers;

namespace ms.rabbitmq.Middlewares
{
    public class UseRabbitConsumer : IHostedService
    {
        private readonly IConsumer consumer;

        public UseRabbitConsumer(IConsumer consumer)
        {
            this.consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            consumer.Subscribe();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
